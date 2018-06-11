using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Photo
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name
        {
            set;get;
        }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Path")]
        public string Path { set; get; }
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Date")]
        public DateTime Date { set; get; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string Descreption { set; get; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ThumbnailPath")]
        public string ThumbnailPath { set; get; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "TwoSlashPath")]
        public string TwoSlashPath
        {
            get
            {
                string newPath = Path.Replace(@"\", "\\");
                return newPath;
            }
           
            
        }

    }
}