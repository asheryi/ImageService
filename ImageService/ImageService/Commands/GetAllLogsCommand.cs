using SharedResources.Logging;
using ImageService.Comunication;
using System;
using System.Collections.Generic;
using SharedResources.Communication;
using SharedResources.Commands;
using System.Net.Sockets;
using SharedResources;
using System.Diagnostics;
using ImageService.Logging;
using System.Net;

namespace ImageService.Commands
{
    public class GetAllLogsCommand : ICommand
    {
      
        SingletonServer singletonServer;
        private ICollection<TcpClient> logsClients;
        private ICollection<Log> logs;
        private IMessageGenerator replyGenerator;
        private ILoggingService logger;
        public string Execute(string[] args, out bool result)
        {
           // IPAddress address = IPAddress.Parse(args[0]);
            //int port = int.Parse(args[1]);
           // IPEndPoint p = new IPEndPoint(address, port);
            singletonServer.SendToClient(this.replyGenerator.Generate(CommandEnum.GetAllLogsCommand,logs), new ClientID(args));
            result = true;
            return "ok";
        }
        public GetAllLogsCommand(ICollection<Log> logs,IMessageGenerator generator, ILoggingService logger)
        {
            singletonServer = SingletonServer.Instance;
            logsClients = new List<TcpClient>();
            this.logger = logger;
            this.logs = logs;
            this.replyGenerator = generator;

        }
    }
}
