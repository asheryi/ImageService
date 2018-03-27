using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using System;
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

        public ImageServer(string[] paths)
        {
            foreach (string path in paths)
            {
                IDirectoryHandler handler = new DirectoyHandler(m_controller, m_logging, path);
                CommandRecieved += handler.OnCommandRecieved;
                handler.DirectoryClose += Handler_DirectoryClose; 
            }
        }

        private void Handler_DirectoryClose(object sender, DirectoryCloseEventArgs e)
        {
            CommandRecieved -= ((IDirectoryHandler)sender).OnCommandRecieved;
            m_logging.Log(e.Message,Logging.Modal.MessageTypeEnum.INFO);
        }

        private void terminate()
        {
            CommandRecieved?.Invoke(this,new CommandRecievedEventArgs( (int)(CommandEnum.CloseCommand),null,"*"));

            int x = CommandEnum[0];
        }
    }
}
