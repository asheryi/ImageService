using SharedResources.Logging;
using ImageService.Comunication;
using System.Collections.Generic;
using SharedResources.Communication;
using SharedResources.Commands;
using System.Net.Sockets;
using ImageService.Logging;

namespace ImageService.Commands
{
    public class GetAllLogsCommand : ICommand
    {
      
        SingletonServer singletonServer;
        private ICollection<TcpClient> logsClients;
        private ICollection<Log> logs;
        private IMessageGenerator replyGenerator;

        public string Execute(string[] args, out bool result)
        {
            // Send to the connected GUI according to the specific protocol the collection of logs.
            singletonServer.SendToClient(this.replyGenerator.Generate(CommandEnum.GetAllLogsCommand,logs), new ClientID(args));
            result = true;
            return "ok";
        }
        public GetAllLogsCommand(ICollection<Log> logs,IMessageGenerator generator)
        {
            singletonServer = SingletonServer.Instance;
            logsClients = new List<TcpClient>();
            this.logs = logs;
            replyGenerator = generator;
        }
    }
}
