using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller.Handlers
{
    class HandlersManager
    {



        private IDictionary<string, IDirectoryHandler> handlers;
        public HandlersManager()
        {
            handlers = new Dictionary<string, IDirectoryHandler>();
        }

        public void Add(string handlerPath,IDirectoryHandler handler)
        {
            handlers[handlerPath] = handler;
        }

        public IDirectoryHandler Remove(string handlerPath)
        {
            if (!handlers.Keys.Contains(handlerPath))
                return null;

            IDirectoryHandler handler = handlers[handlerPath];

            handlers.Remove(handlerPath);

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
