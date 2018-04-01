using ImageService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        private const string m_thumsNailOutputFolder = "ThumbsNail";
        #endregion

        public ImageServiceModal(string outputFolder,int thumbnailSize)
        {
            this.m_OutputFolder = outputFolder;
            this.m_thumbnailSize = thumbnailSize;

            CreateFolder(Path.Combine(m_OutputFolder, m_thumsNailOutputFolder));
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

                DateTime creationTime = File.GetLastWriteTime(path);

            
                int year = creationTime.Year;
                int month = creationTime.Month;
                string dstPath = m_OutputFolder + @"\" + year + @"\" + month;            
                   CreateFolder(dstPath);
                MoveFile(path, dstPath);
                //result = false;
                //return dstPath;

               // string fileNmae=fileInfo.Name.Replace('')
                Image image = Image.FromFile(dstPath+@"\"+fileInfo.Name);
                Image thumb = image.GetThumbnailImage(m_thumbnailSize, m_thumbnailSize, () => false, IntPtr.Zero);
                
                dstPath = m_OutputFolder + @"\" + m_thumsNailOutputFolder + @"\" + year + @"\" + month; 
                CreateFolder(dstPath);

                thumb.Save(Path.ChangeExtension(dstPath+ @"\" + fileInfo.Name, fileInfo.Extension));



                result = true;

                return dstPath;
            } catch(IOException e)
            {
                result = false;
                return e.ToString();
            }


        }

        private void CreateFolder(string targetPath)
        {
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }
        }

        private void MoveFile(string srcPathWithFileName,string dstPath)
        {
            File.Move(srcPathWithFileName, Path.Combine(dstPath, Path.GetFileName(srcPathWithFileName)));
        }



    }
}
