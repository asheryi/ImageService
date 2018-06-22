using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    class Bitmap
    {
        private MemoryStream ms;
        public Bitmap(System.Drawing.Bitmap bitmap,string imgName_)
        {
            Name = imgName_;
            this.img = bitmap;
          //  img = new System.Drawing.Bitmap(ms);
        }
        private System.Drawing.Bitmap img;
        private string imgName;
        public System.Drawing.Bitmap Image
        {
          
            get
            {
                return img;
            }
        }
        public string Name
        {
            set
            {
                imgName = value ;
            }
            get
            {
                return imgName;
            }
        }
        public MemoryStream MemoryStream
        {
            get
            {
                return ms;
            }
            set
            {
                ms = value;
            }
        }
    }
}
