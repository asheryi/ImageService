using ImageService.Logging;
using ImageService.Model;
using SharedResources.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
   public class ImageControllerArgs
    {
       
        private string[] paths;
        public string[] DirectoriesPaths
        {
            set
            {
                paths = value;
            }
            get
            {
                return paths;
            }
        }
        private ILoggingService logger;
        public ILoggingService LoggingService
        {
            set
            {
                logger = value;
            }
            get
            {
                return logger;
            }
        }
        private IImageServiceModel imageServiceModel;
        public IImageServiceModel ImageServiceModel
        {
            set
            {
                imageServiceModel = value;
            }
            get
            {
                return imageServiceModel;
            }
        }
       
       
        private EventHandler<DirectoryCloseEventArgs> handlerDirectoryClose;
       public EventHandler<DirectoryCloseEventArgs> HandlerDirectoryClose
        {
            set
            {
                handlerDirectoryClose = value;
            }
            get
            {
                return handlerDirectoryClose;
            }
        }
        private Action serverDown;
           public Action ServerDown
        {
            set
            {
                serverDown = value;
            }
            get
            {
                return serverDown;
            }
        }
        private ImageServiceModelArgs imageServiceModelArgs;
        public ImageServiceModelArgs ImageServiceModelArgs {
            set
            {
                imageServiceModelArgs = value;
            }
            get
            {
                return imageServiceModelArgs;
            }
        }
        
    }
}
