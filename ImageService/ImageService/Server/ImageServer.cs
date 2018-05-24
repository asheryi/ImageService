using SharedResources.Logging;
using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Logging;
using ImageService.Model;
using System;
using System.Diagnostics;
using SharedResources;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController m_controller; // the controller to the Model
        private ILoggingService m_logger; // the logger
        #endregion

        #region Properties
        public event Action serverDown;

        #endregion

        public ImageServer(ImageServerArgs imageServerArgs,ref EventHandler<Log> LogAnnouncement)
        {
            m_logger = imageServerArgs.LoggingService;
            imageServerArgs.ImageControllerArgs.LoggingService = imageServerArgs.LoggingService;
            imageServerArgs.ImageControllerArgs.HandlerDirectoryClose = Handler_DirectoryClose;
            imageServerArgs.ImageControllerArgs.ServerDown = serverDown;
            m_controller = new ImageController(imageServerArgs.ImageControllerArgs,ref LogAnnouncement,ref serverDown);
           
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
            m_logger.Log(e.Message,MessageTypeEnum.WARNING);
        }



        /// <summary>
        /// Terminates the server and all of the Directory handlers (DH-s)
        /// </summary>
        public void terminate()
        {
            serverDown?.Invoke();
            m_logger.Log("Server Down", MessageTypeEnum.WARNING);
        }
    }
}
