using ImageService.Model;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImageService.Logging;
using SharedResources.Logging;
using SharedResources.Commands;

namespace ImageService.Controller.Handlers
{
    public class DirectoyHandler : IDirectoryHandler
    {
        #region Members
        private delegate void CommandFunction(CommandRecievedEventArgs e);
        private IImageController m_controller; // The Image Processing Controller
        private ILoggingService m_logging;    //the logger of the system. 
        private List<FileSystemWatcher> m_dirWatchers;   // The Watchers of the Dir based on each extension watched
        private string m_path; // The Path of directory
        public delegate string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful);
        private Dictionary<int, CommandFunction> m_Commands;//Dictionary of commands.
        #endregion
        ///
        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;//The Event That Notifies that the Directory is being closed
        /// <summary>
        /// DirectoyHandler constructor.
        /// </summary>
        /// <param name="controller">the Controller Of The System.</param> 
        /// <param name="logging">the Logger Of The System.</param> 
        /// <param name="path">the path that the DirectoyHandler responsible for.</param>
        public DirectoyHandler(IImageController controller, ILoggingService logging, string path)
        {
            m_controller = controller;
            m_logging = logging;
            m_path = path;
            string[] filters = new string[] { "*.jpg", "*.png", "*.gif", "*.bmp" };

            m_Commands = new Dictionary<int, CommandFunction>();
            m_Commands.Add((int)(CommandEnum.CloseHandlerCommand), closeCommand);

            m_dirWatchers = new List<FileSystemWatcher>(filters.Length);

            foreach (string filter in filters)
            {
                FileSystemWatcher f = new FileSystemWatcher(m_path);
                f.Filter = filter;
                //subscribe to the event of new file added to the watching folder.
                f.Created += new FileSystemEventHandler(OnCreated);
                m_dirWatchers.Add(f);
            }

        }
        /// <summary>
        /// closeCommand close DirectoyHandler.
        /// </summary>
        /// <param name="e">arguments for the function</param> 
        public void closeCommand(CommandRecievedEventArgs e)
        {
            //closing all the watchers before close the DirectoyHandler.
            foreach (FileSystemWatcher watcher in m_dirWatchers)
            {
                watcher.EnableRaisingEvents = false;
            }

                
         //  DirectoryClose?.Invoke(this, new DirectoryCloseEventArgs(e.RequestDirPath, m_path + " is closed for buisness ."));
        }
        /// <summary>
        /// StartHandleDirectory start watching.
        /// </summary>
        public void StartHandleDirectory()
        {
            // Begin watching.
            foreach (FileSystemWatcher watcher in m_dirWatchers)
            {
                watcher.EnableRaisingEvents = true;
            }
        }

        



        /// <summary>
        /// OnCreated new file in the watch folder.
        /// </summary>
        /// <param name="source">the object who call the function.</param> 
        /// <param name="e">arguments for the function.</param> 
        private void OnCreated(object source, FileSystemEventArgs e)
        {

            m_logging.Log("FILE DETECTED : " + e.Name, MessageTypeEnum.INFO);
            
            

            bool succeed;
            string message =  m_controller.ExecuteCommand((int)CommandEnum.NewFileCommand, new string[] { e.FullPath },out succeed);

            //getting the Task result.
            if (!succeed)
            {
                m_logging.Log(message, MessageTypeEnum.FAIL);
            }
            else
            {
                m_logging.Log("Succeeded to move the file " +e.Name +" to :  " +message, MessageTypeEnum.INFO);
            }
        }

        public void Close()
        {
            //closing all the watchers before close the DirectoyHandler.
            foreach (FileSystemWatcher watcher in m_dirWatchers)
            {
                watcher.EnableRaisingEvents = false;
            }


            DirectoryClose?.Invoke(this, new DirectoryCloseEventArgs(m_path + " is closed for buisness ."));
        }
    }
}
