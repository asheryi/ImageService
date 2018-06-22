using ImageService.Logging;
using ImageService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller.Handlers
{
   public class HandlersManager
    {



        private IDictionary<string, IDirectoryHandler> handlers;
        private object handlersLock = new object();
        public delegate string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful);
        /// <summary>
        /// HandlersManager constructor.
        /// </summary>
        /// <param name="m_controller">Controller of the service</param>
        /// <param name="paths">Paths of the folder to be watch</param>
        /// <param name="logger">Logging</param>
        /// <param name="closeHandler">Close handler EventHandler</param>
        /// <param name="serverDown">serverDown Action</param>
        public HandlersManager(IImageController m_controller,string[] paths,ILoggingService logger,EventHandler<DirectoryCloseEventArgs> closeHandler,ref Action serverDown)
        {
            handlers = new Dictionary<string, IDirectoryHandler>();
            for (int i = 0; i < paths.Length; i++)
            {
                string path = paths[i];
                IDirectoryHandler handler = new DirectoyHandler(m_controller, logger, path);
                Add(path, handler);
                handler.DirectoryClose += closeHandler;
                serverDown += handler.Close;
                handler.StartHandleDirectory();
            }
        }
        /// <summary>
        /// HandlersPath collection property
        /// </summary>
        /// <returns></returns>
       public IList<string> getHandlersPaths()
        {
                lock (handlersLock)
                {
                    return handlers.Keys.ToList();
                }
        }
        /// <summary>
        /// Add DirectoryHandler to the dictionary.
        /// </summary>
        /// <param name="handlerPath">path of the handler</param>
        /// <param name="handler">IDirectoryHandler handler</param>
        public void Add(string handlerPath,IDirectoryHandler handler)
        {
            lock (handlersLock)
            {
                handlers[handlerPath] = handler;
            }
        }
        /// <summary>
        /// Remove DirectoryHandler.
        /// </summary>
        /// <param name="handlerPath">Path of the DirectoryHandler to be remove.</param>
        /// <returns>The removed DirectoryHandler.</returns>
        public IDirectoryHandler Remove(string handlerPath)
        {
            IDirectoryHandler handler;
            lock (handlersLock)
            {
                if (!handlers.Keys.Contains(handlerPath))
                    return null;

                 handler = handlers[handlerPath];

                handlers.Remove(handlerPath);
            }
            return handler;
        }
      
    }
}
