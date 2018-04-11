using ImageService.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    public class ImageServiceModal : IImageServiceModal
    {
        #region Members
        private string m_OutputFolder;            // The Output Folder
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size
        private ILoggingService m_logger;
        private const string m_thumsNailOutputFolder = "ThumbsNail";
        private const string m_undefinedTakenDate = "Undefined-dates images";

        #endregion

        public ImageServiceModal(ILoggingService logger,string outputFolder,int thumbnailSize)
        {
            this.m_OutputFolder = outputFolder;
            this.m_thumbnailSize = thumbnailSize;
            this.m_logger = logger;
            CreateFolder(Path.Combine(m_OutputFolder, m_thumsNailOutputFolder));
            CreateFolder(Path.Combine(m_OutputFolder, m_undefinedTakenDate));
        }


        static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open,
                         FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        public string AddFile(string path, out bool result)
        {
            try
            {
                //if (!File.Exists(path))
                //{
                //    result = false;
                //    return "File Not Exists!";
                //}

                FileInfo fileInfo = new FileInfo(path);
                while (IsFileLocked(fileInfo))
                {
                    Thread.Sleep(500);
                }

                string dstPath, thumbsDstPath;

                try
                {
                    DateTime creationTime = GetDateTakenFromImage(path);


                    int year = creationTime.Year;
                    int month = creationTime.Month;
                    dstPath = m_OutputFolder + @"\" + year + @"\" + month;
                    CreateFolder(dstPath);

                    thumbsDstPath = m_OutputFolder + @"\" + m_thumsNailOutputFolder + @"\" + year + @"\" + month;
                } catch(ArgumentException e)
                {
                    dstPath = m_OutputFolder + @"\" + m_undefinedTakenDate;
                    thumbsDstPath = m_OutputFolder + @"\" + m_thumsNailOutputFolder + @"\" + m_undefinedTakenDate;
                }


                string newName = GenerateFileName(fileInfo, dstPath, thumbsDstPath);

                MoveFile(path, dstPath,newName);
                //result = false;
                //return dstPath;

                // string fileNmae=fileInfo.Name.Replace('')
                Image image  = Image.FromStream(new MemoryStream(File.ReadAllBytes(dstPath + @"\" + newName)));
                //Image image = Image.FromFile(dstPath+@"\"+ newName);
                
                Image thumb = image.GetThumbnailImage(m_thumbnailSize, m_thumbnailSize, () => false, IntPtr.Zero);
                
                CreateFolder(thumbsDstPath);

                thumb.Save(Path.ChangeExtension(thumbsDstPath + @"\" + newName, fileInfo.Extension));



                result = true;

                return dstPath;
            } catch(IOException e)
            {
                result = false;
                return e.ToString();
            }


        }
        private string GenerateFileName(FileInfo fileInfo,string orgSizePath,string thumbnailPath)
        {
            string fileName = Path.GetFileNameWithoutExtension(fileInfo.Name);
            string extension = fileInfo.Extension;
            

            m_logger.Log(orgSizePath + @"\" + fileName, Logging.Modal.MessageTypeEnum.FAIL);
            m_logger.Log(thumbnailPath + @"\" + fileName, Logging.Modal.MessageTypeEnum.FAIL);

            if (!File.Exists(orgSizePath + @"\" +fileInfo.Name) && !File.Exists(thumbnailPath + @"\" + fileInfo.Name))
                return fileInfo.Name;

            int fileIndex_ = 1;
            while (File.Exists(orgSizePath + @"\" + fileName+ "("+fileIndex_+")" + extension) || File.Exists(thumbnailPath + @"\" + fileName + "(" + fileIndex_ + ")" + extension))
            {
                fileIndex_++;
            }
            return fileName + "(" + fileIndex_ + ")" + extension;
        }
        private void CreateFolder(string targetPath)
        {
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }
        }

        private void MoveFile(string srcPathWithFileName,string dstPath,string newName)
        {
            File.Move(srcPathWithFileName, Path.Combine(dstPath,newName));

        }

        //we init this once so that if the function is repeatedly called
        //it isn't stressing the garbage man
        private static Regex r = new Regex(":");

        //retrieves the datetime WITHOUT loading the whole image
        private DateTime GetDateTakenFromImage(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (Image myImage = Image.FromStream(fs, false, false))
            {
                PropertyItem propItem = myImage.GetPropertyItem(36867);
                string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                return DateTime.Parse(dateTaken);
            }
        }

    }
}
