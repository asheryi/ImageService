using ImageService.Comunication;
using ImageService.Controller.Handlers;
using SharedResources;
using SharedResources.Commands;
using SharedResources.Communication;
using System;

namespace ImageService.Commands
{
    class CloseHandlerCommand : ICommand
    {
        private HandlersManager handlersManager; 
        /// <summary>
        /// Recieves the handlers manager.
        /// </summary>
        /// <param name="handlersManager"></param>
        public CloseHandlerCommand(HandlersManager handlersManager)
        {
            this.handlersManager = handlersManager;
        }

        public string Execute(string[] args, out bool result)
        {
            // Removes and closes the handler.
            IDirectoryHandler handler = handlersManager.Remove(args[0]);
            result = true;
            if (handler == null)
            {
                result = false;
                return "";
            }

            try
            {
                handler.Close();
            }catch(Exception)
            {
                result = false;
            }

            if (result)
            {
                SingletonServer singletonServer = SingletonServer.Instance;
                CommunicationMessageGenerator CommunicationMessageGenerator = new CommunicationMessageGenerator();
                singletonServer.SendToAll(CommunicationMessageGenerator.Generate(CommandEnum.CloseHandlerCommand,new DirectoryDetails(args[0])));
            }
            return args[0];
        }
    }
}
