using ImageService.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Model
{
   public class ImageServiceModelArgs
    {
        private ILoggingService loggingService;
        private string managePath;
        private int thumbailSize;
        public ILoggingService LoggingService
        {
            set
            {
                loggingService = value;
            }
            get
            {
                return loggingService;
            }
        }
        public string ManagePath
        {
            set
            {
                managePath = value;
            }
            get
            {
                return managePath;
            }
        }
        public int ThumbnailsSize
        {
            set
            {
                thumbailSize = value;
            }
            get
            {
                return thumbailSize;
            }
        }
    }
}
