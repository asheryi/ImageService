using ImageService.Controller.Handlers;
using System;

namespace ImageService.Commands
{
    class CloseHandlerCommand : ICommand
    {
        private HandlersManager handlersManager; 

        public CloseHandlerCommand(HandlersManager handlersManager)
        {
            this.handlersManager = handlersManager;
        }

        public string Execute(string[] args, out bool result)
        {
            IDirectoryHandler handler = handlersManager.Remove(args[0]);
            if(handler == null)
            {
                result = false;
                return "";
            }

            // ZARICH EIUN

            handler.Close();

            result = true;
            return args[0];
        }
    }
}
