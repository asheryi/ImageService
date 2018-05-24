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

        public CloseHandlerCommand(HandlersManager handlersManager)
        {
            this.handlersManager = handlersManager;
        }

        public string Execute(string[] args, out bool result)
        {
            IDirectoryHandler handler = handlersManager.Remove(args[0]);
            result = true;
            if (handler == null)
            {
                result = false;
                return "";
            }

            // ZARICH EIUN
            try
            {
                handler.Close();
            }catch(Exception e)
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
