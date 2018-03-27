using ImageService.Infrastructure;
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
        #endregion

        public ImageServiceModal(string outputFolder,int thumbnailSize)
        {
            this.m_OutputFolder = outputFolder;
            this.m_thumbnailSize = thumbnailSize;

            CreateFolder(Path.Combine(m_OutputFolder, "ThumbsNail"));
        }



        public string AddFile(string path, out bool result)
        {
            try
            {
                if (!File.Exists(path))
                {
                    result = false;
                    return "File Not Exists!";
                }

                DateTime creationTime = File.GetLastWriteTime(path);

            
                int year = creationTime.Year;
                int month = creationTime.Month;
                string dstPath = m_OutputFolder + "/" + year + "/" + month;            
                   CreateFolder(dstPath);
                MoveFile(path, dstPath);





                result = true;
            } catch(IOException e)
            {
                result = false;
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
