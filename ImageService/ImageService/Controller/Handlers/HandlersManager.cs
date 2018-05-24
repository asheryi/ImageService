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
        public HandlersManager(IImageController m_controller,string[] paths,ILoggingService logger,EventHandler<DirectoryCloseEventArgs> Handler_DirectoryClose, Action serverDown)
        {
            handlers = new Dictionary<string, IDirectoryHandler>();
            for (int i = 0; i < paths.Length; i++)
            {
                string path = paths[i];
                IDirectoryHandler handler = new DirectoyHandler(m_controller, logger, path);
                Add(path, handler);
                handler.DirectoryClose += Handler_DirectoryClose;
                serverDown += handler.Close;
                handler.StartHandleDirectory();
            }
        }
       public ICollection<string> getHandlersPaths()
        {
                lock (handlersLock)
                {

                    return handlers.Keys;
                }
        }
        public void Add(string handlerPath,IDirectoryHandler handler)
        {
            lock (handlersLock)
            {
                handlers[handlerPath] = handler;
            }
        }

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

        public void killAllHandlers()
        {
            foreach(string key in handlers.Keys)
            {
                handlers[key].Close();
            }
        }
      
    }
}
