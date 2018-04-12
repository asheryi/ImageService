using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using System;
using System.Collections.Generic;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController m_controller; // the controller to the modal
        private ILoggingService m_logger; // the logger
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        #endregion

        public ImageServer(string[] paths,ILoggingService logger,IImageServiceModal modal)
        {
            m_controller = new ImageController(modal);

            // Creating list of handlers
            List<IDirectoryHandler> handlers = new List<IDirectoryHandler>();

            for(int i=0;i<paths.Length;i++) 
            {
                string path = paths[i];
                handlers.Add(new DirectoyHandler(m_controller, logger, path));
                    
                CommandRecieved += handlers[i].OnCommandRecieved;
                handlers[i].DirectoryClose += Handler_DirectoryClose;
                handlers[i].StartHandleDirectory();
            }

            m_controller = new ImageController(modal);
            m_logger = logger;

        }



        /// <summary>
        /// Registered function to the event of IDirectoryHandler that happens
        /// in the closing of a DH .
        /// </summary>
        /// <param name="sender">the sender object in this case DH</param>
        /// <param name="e">The args</param>
        private void Handler_DirectoryClose(object sender, DirectoryCloseEventArgs e)
        {
            // unsubsribe the DH from the event of CommandRecieved .
            CommandRecieved -= ((IDirectoryHandler)sender).OnCommandRecieved;
            // Logging the message of the closed directory
            m_logger.Log(e.Message,Logging.Modal.MessageTypeEnum.INFO);
        }



        /// <summary>
        /// Terminates the server and all of the Directory handlers (DH-s)
        /// </summary>
        public void terminate()
        {
            CommandRecieved?.Invoke(this,new CommandRecievedEventArgs( (int)(CommandEnum.CloseCommand),null,"*"));
            m_logger.Log("Server Down", Logging.Modal.MessageTypeEnum.WARNING);
        }
    }
}
