using ImageService.Modal;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using System.Text.RegularExpressions;

namespace ImageService.Controller.Handlers
{
    public class DirectoyHandler : IDirectoryHandler
    {
        #region Members
        private IImageController m_controller;              // The Image Processing Controller
        private ILoggingService m_logging;
        private List<FileSystemWatcher> m_dirWatchers;             // The Watchers of the Dir based on each extension watched
        private string m_path; // The Path of directory
        #endregion

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed

        public DirectoyHandler(IImageController controller, ILoggingService logging, string path)
        {
            m_controller = controller;
            m_logging = logging;
            m_path = path;
            m_dirWatchers = new List<FileSystemWatcher>();

            string[] filters = new string[] { "*.jpg", "*.png", "*.gif", "*.bmp" };
            foreach (string filter in filters)
            {
                FileSystemWatcher f = new FileSystemWatcher(m_path);
                f.Filter = filter;
                f.Created += new FileSystemEventHandler(OnCreated);
                m_dirWatchers.Add(f);
                m_logging.Log("Filtering " + filter, MessageTypeEnum.INFO);
            }

        }

        public void StartHandleDirectory(string dirPath)
        {



            // Begin watching.
            foreach (FileSystemWatcher watcher in m_dirWatchers)
            {
                watcher.EnableRaisingEvents = true;
            }

        }

        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            if (e.RequestDirPath!="*" && e.RequestDirPath != this.m_path)
            {
                return;
            }

            if(e.CommandID == (int)(CommandEnum.CloseCommand))
            {
                foreach(FileSystemWatcher watcher in m_dirWatchers)
                {
                    watcher.EnableRaisingEvents = false;
                    DirectoryClose?.Invoke(this,new DirectoryCloseEventArgs(e.RequestDirPath,"Directory closed"));
                }

            }
        }

        

        
        private void OnCreated(object source, FileSystemEventArgs e)
        {

            m_logging.Log("FILE DETECTED", Logging.Modal.MessageTypeEnum.INFO);


            bool succeed;

            Task<string> commandTask = new Task<string>(() => { return m_controller.ExecuteCommand((int)CommandEnum.NewFileCommand, new string[] { e.FullPath }, out succeed); });

            commandTask.Start();
            string message = commandTask.Result;
            if (true)//!succeed) ////////////////////////////////////////
            {
                m_logging.Log(message, MessageTypeEnum.FAIL);
            }
        }











        // Implement Here!
    }
}
