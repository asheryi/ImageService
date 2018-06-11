using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using SharedResources;

namespace WebApplication2.Models
{
    public class PhotosModel
    {
        ICollection<Photo> photos;
        public ICollection<Photo> Photos
        {
            set
            {
                photos = value;
            }
            get
            {
                return photos;
            }
        }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ThumbnailSize")]
        public int ThumbnailSize { get; set; }
        private string managePath;
        string fullPath;
        private string manageRelativePath;
        private string thumbnailFullPath;
        public PhotosModel(string managePath,int thumbnailSize)
        {
            fullPath = managePath;
            string serverLocalPath = HttpContext.Current.Server.MapPath("~");
            manageRelativePath = managePath.Substring(serverLocalPath.Length-1,managePath.Length-serverLocalPath.Length+1);
           
            ThumbnailSize = thumbnailSize;
            photos = new List<Photo>();
            string pathOfThubNail = this.managePath + @"\ThumbsNail";
            thumbnailFullPath = managePath + @"\ThumbsNail";
            string[] extensions = new string[] { "*.jpg", "*.png", "*.gif", "*.bmp" };
            List<string> allfiles = new List<string>();
            foreach(string extension in extensions)
            {
                allfiles.AddRange(Directory.GetFiles(thumbnailFullPath, extension, SearchOption.AllDirectories).ToList<string>());

            }
            fillListPhoto(allfiles);
            
        }
        private void fillListPhoto(ICollection<string> allfiles)
        {
            
            foreach (string image in allfiles)
            {
                string relativePath = image.Remove(0, thumbnailFullPath.Length);
                Photo photo = new Photo();
                photo.Name = Path.GetFileName(image);
                photo.ThumbnailPath = manageRelativePath+@"\ThumbsNail" + relativePath;
                photo.Path = manageRelativePath + relativePath;
              
                photo.Date = GetPhotoDate(fullPath + relativePath);
                Debug.WriteLine(photo.Date);
                photos.Add(photo);
            }
        }
        //public void UpdatePhotosList()
        //{
           
        //    string pathOfThubNail = managePath + @"\ThumbsNail";
        //    string[] extensions = new string[] { "*.jpg", "*.png", "*.gif", "*.bmp" };
        //    List<string> allfiles = new List<string>();
        //    foreach (string extension in extensions)
        //    {
        //        allfiles.AddRange(Directory.GetFiles(pathOfThubNail, extension, SearchOption.AllDirectories).ToList<string>());

        //    }
        //    List<string> newFiles = new List<string>();
        //    bool exist = false;
        //    foreach (string image in allfiles)
        //    {
        //        foreach(Photo photo in photos)
        //        {
        //            if (photo.ThumbnailPath == image)
        //            {
        //                exist = true;
        //            }
        //        }
        //        if(!exist)
        //        newFiles.Add(image);
        //        exist = false;
        //    }

        //    fillListPhoto(newFiles, managePath, pathOfThubNail);
        //}
        private DateTime GetPhotoDate(string path)
        {
            DateTime TakenDateTime=new DateTime();
            try
            {
                 TakenDateTime = GetDateTakenFromImage(path);
                return TakenDateTime;
            }
            catch (ArgumentException)
            {
                return new DateTime(1, 1, 1);
            }
           
        }

        public void newFileRecieved(object sender, ContentEventArgs e)
        {
            string newFilePath = e.GetContent<string>();
            fillListPhoto(new string[] { newFilePath });

        }

        private static Regex r = new Regex(":");
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

        internal void DeleteImage(string path)
        {
            string thumbnailPath="";
         
           foreach(Photo photo in photos)
            {
                if (photo.Path == path)
                {
                    thumbnailPath = photo.ThumbnailPath;
                    photos.Remove(photo);
                    break;
                }
            }
            string serverPath = HttpContext.Current.Server.MapPath("~");
            try
            {
                //string path_ = path.Substring(1);
                File.Delete(serverPath + path);
                //path = thumbnailPath.Substring(2);
                File.Delete(serverPath + thumbnailPath);
            }
            catch
            {

            }

        }
    }
}