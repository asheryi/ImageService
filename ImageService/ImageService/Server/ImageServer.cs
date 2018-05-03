using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Model;
using System;
using System.Collections.Generic;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController m_controller; // the controller to the Model
        private ILoggingService m_logger; // the logger
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved

        public event Action serverDown;

        #endregion

        public ImageServer(string[] paths,ILoggingService logger,IImageServiceModel Model)
        {
            HandlersManager handlers = new HandlersManager();
            // Creating list of handlers

            for(int i=0;i<paths.Length;i++) 
            {
                string path = paths[i];
                IDirectoryHandler handler = new DirectoyHandler(m_controller, logger, path);
                handlers.Add(path,handler);
                    
                //CommandRecieved += handler.OnCommandRecieved;
                handler.DirectoryClose += Handler_DirectoryClose;
                serverDown += handler.Close;
                handler.StartHandleDirectory();
            }            

            m_controller = new ImageController(Model,handlers);
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
            serverDown -= ((IDirectoryHandler)sender).Close;
            // Logging the message of the closed directory
            m_logger.Log(e.Message,Logging.Model.MessageTypeEnum.INFO);
        }



        /// <summary>
        /// Terminates the server and all of the Directory handlers (DH-s)
        /// </summary>
        public void terminate()
        {
            //CommandRecieved?.Invoke(this,new CommandRecievedEventArgs( (int)(CommandEnum.CloseCommand),null,"*"));

            serverDown?.Invoke();
            m_logger.Log("Server Down", Logging.Model.MessageTypeEnum.WARNING);
        }
    }
}
