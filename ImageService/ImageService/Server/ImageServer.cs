using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        #endregion

        public ImageServer(string[] paths,ILoggingService logger,IImageServiceModal modal)
        {
            m_controller = new ImageController(modal);

            logger.Log("before create Directory", Logging.Modal.MessageTypeEnum.INFO);

            List<IDirectoryHandler> handlers = new List<IDirectoryHandler>();

            for(int i=0;i<paths.Length;i++) 
            {
                string path = paths[i];
                logger.Log("creating Directory handler in server", Logging.Modal.MessageTypeEnum.INFO);
                handlers.Add(new DirectoyHandler(m_controller, logger, path));
                    
                CommandRecieved += handlers[i].OnCommandRecieved;
                handlers[i].DirectoryClose += Handler_DirectoryClose;
                handlers[i].StartHandleDirectory();
                
            }
            logger.Log("after create Directory", Logging.Modal.MessageTypeEnum.INFO);


            m_controller = new ImageController(modal);
            m_logging = logger;

            m_logging.Log("Created Server", Logging.Modal.MessageTypeEnum.INFO);

        }

        private void Handler_DirectoryClose(object sender, DirectoryCloseEventArgs e)
        {
            CommandRecieved -= ((IDirectoryHandler)sender).OnCommandRecieved;
            m_logging.Log(e.Message,Logging.Modal.MessageTypeEnum.INFO);
        }

        public void terminate()
        {
            CommandRecieved?.Invoke(this,new CommandRecievedEventArgs( (int)(CommandEnum.CloseCommand),null,"*"));
            m_logging.Log("Server Down", Logging.Modal.MessageTypeEnum.WARNING);
        }
    }
}
