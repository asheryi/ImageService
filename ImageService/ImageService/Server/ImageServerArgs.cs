﻿using ImageService.Controller;
using ImageService.Logging;
using ImageService.Logging.Model;
using ImageService.Model;
using SharedResources.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
   public class ImageServerArgs
    {
        
        public EventHandler<Log> LogAnnouncement{
            get;
            set;
        }
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
       private  ILoggingService logger;
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
        private ImageService imageService;
        public ImageService ImageService
        {
            get
            {
                return imageService;
            }
            set
            {
                imageService = value;
            }
        }
        private ImageControllerArgs imageControllerArgs;
        public ImageControllerArgs ImageControllerArgs
        {
            get
            {
                return imageControllerArgs;
            }
            set
            {
                imageControllerArgs = value;
            }
        }
        //public ImageServerArgs(string[] paths, ILoggingService logger, IImageServiceModel Model, ImageService service, EventLog eventLogger)
        //{

        //}
    }
}
