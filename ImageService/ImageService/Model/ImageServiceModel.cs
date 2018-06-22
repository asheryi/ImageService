using ImageService.Logging;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ImageService.Model
{
    public class ImageServiceModel : IImageServiceModel
    {
        #region Members
        private string m_OutputFolder;            // The Output Folder
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size
        private ILoggingService m_logger;         // the logger
        private const string m_thumsNailOutputFolder = "ThumbsNail"; // Thumbnails folder name
        private const string m_undefinedTakenDate = "Undefined-dates images"; // Outcasts folder (without photo date)

        #endregion

        public ImageServiceModel(ILoggingService logger,string outputFolder,int thumbnailSize)
        {
            this.m_OutputFolder = outputFolder;
            this.m_thumbnailSize = thumbnailSize;
            this.m_logger = logger;
            CreateFolder(Path.Combine(m_OutputFolder, m_thumsNailOutputFolder));
            CreateFolder(Path.Combine(m_OutputFolder, m_undefinedTakenDate));
        }

        public ImageServiceModel(ImageServiceModelArgs imageServiceModelArgs)
        {
            this.m_OutputFolder = imageServiceModelArgs.ManagePath;
            this.m_thumbnailSize = imageServiceModelArgs.ThumbnailsSize;
            this.m_logger = imageServiceModelArgs.LoggingService;
            CreateFolder(Path.Combine(m_OutputFolder, m_thumsNailOutputFolder));
            CreateFolder(Path.Combine(m_OutputFolder, m_undefinedTakenDate));
        }

        /// <summary>
        /// Check if a file is already done downloading .
        /// </summary>
        /// <param name="file">file info</param>
        /// <returns>true iff  file is not done downloading</returns>
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

        /// <summary>
        /// The AddFile function is the main function to add a file (with a given
        /// path ) to the specific directoy (according to the taken date of the photo).
        /// </summary>
        /// <param name="path">path of the file</param>
        /// <param name="result">updated to true iff succeded</param>
        /// <returns>string of failure in case of failure , and path in case of success</returns>
        public string AddFile(string path, out bool result)
        {
            try
            {
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
                } catch(ArgumentException)
                {
                    dstPath = m_OutputFolder + @"\" + m_undefinedTakenDate;
                    thumbsDstPath = m_OutputFolder + @"\" + m_thumsNailOutputFolder + @"\" + m_undefinedTakenDate;
                }


                string newName = GenerateFileName(fileInfo, dstPath, thumbsDstPath);

                MoveFile(path, dstPath,newName);

                Image image  = Image.FromStream(new MemoryStream(File.ReadAllBytes(dstPath + @"\" + newName)));
                Image thumb = image.GetThumbnailImage(m_thumbnailSize, m_thumbnailSize, () => false, IntPtr.Zero);
                
                CreateFolder(thumbsDstPath);
                string thumbPath = Path.ChangeExtension(thumbsDstPath + @"\" + newName, fileInfo.Extension);
                thumb.Save(thumbPath);
                result = true;

                return dstPath+";"+ thumbPath;
            } catch(IOException e)
            {
                result = false;
                return e.ToString();
            }
        }

        /// <summary>
        /// Generates a file name to be in the output folder .
        /// 
        /// It's purpose is to handle the case of multiple files with same name
        /// so a we scan for the first index which the same file with the index
        /// afterwards doesn't exsists in both the output folder , and in the 
        /// Thumbnails folder .
        /// </summary>
        /// <param name="fileInfo">the file info .</param> : 
        /// <param name="orgSizePath">the original photo path</param> 
        /// <param name="thumbnailPath">the thumbnail to be path .</param> - 
        /// <returns>the name to be in the output folder.</returns> 
        private string GenerateFileName(FileInfo fileInfo,string orgSizePath,string thumbnailPath)
        {
            string fileName = Path.GetFileNameWithoutExtension(fileInfo.Name);
            string extension = fileInfo.Extension;

            if (!File.Exists(orgSizePath + @"\" +fileInfo.Name) && !File.Exists(thumbnailPath + @"\" + fileInfo.Name))
                return fileInfo.Name;

            int fileIndex_ = 1;
            while (File.Exists(orgSizePath + @"\" + fileName+ "("+fileIndex_+")" + extension) || File.Exists(thumbnailPath + @"\" + fileName + "(" + fileIndex_ + ")" + extension))
            {
                fileIndex_++;
            }
            return fileName + "(" + fileIndex_ + ")" + extension;
        }

        /// <summary>
        /// Creates a folder , if it doesn't exsists aleready.
        /// </summary>
        /// <param name="targetPath">path of the folder to create</param>
        private void CreateFolder(string targetPath)
        {
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }
        }

        /// <summary>
        /// Moves a file from src to dst with a new name .
        /// </summary>
        /// <param name="srcPathWithFileName"> source file path with the file name</param>
        /// <param name="dstPath"> destination path</param>
        /// <param name="newName">new name of the file</param>
        private void MoveFile(string srcPathWithFileName,string dstPath,string newName)
        {

           
            File.Move(srcPathWithFileName, Path.Combine(dstPath,newName));
        
        }
       
        /**********This is used to take the date of the picture taken*********/

        //we init this once so that if the function is repeatedly called
        //it isn't stressing the garbage man
        private static Regex r = new Regex(":");
        private ImageServiceModelArgs imageServiceModelArgs;

        //retrieves the datetime WITHOUT loading the whole image
        public DateTime GetDateTakenFromImage(string path)
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
