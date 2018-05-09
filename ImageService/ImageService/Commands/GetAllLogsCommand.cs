using SharedResources.Logging;
using ImageService.Comunication;
using System;
using System.Collections.Generic;
using SharedResources.Communication;
using SharedResources.Commands;
using System.Net.Sockets;
using SharedResources;
using System.Diagnostics;

namespace ImageService.Commands
{
    class GetAllLogsCommand : ICommand
    {
      
        SingletonServer singletonServer;
        private ICollection<TcpClient> logsClients;
        private ICollection<Log> logs;
        private EventLog eventlogger;
        private IReplyGenerator replyGenerator;

        public string Execute(string[] args, out bool result)
        {
            int clientID = Int32.Parse(args[0]);
            //string allLogsString = ObjectConverter<ICollection<Log>>.Serialize(logs);
            //ServiceReply sr = new ServiceReply(CommandEnum.GetAllLogsCommand, allLogsString);



            singletonServer.SendToClient(this.replyGenerator.Generate(CommandEnum.GetAllLogsCommand,logs), clientID);

            //foreach (Log log in logs)
            //{
                
            //    ServiceReply sr = new ServiceReply(CommandEnum.SendLog, ObjectConverter<Log>.Serialize(log));
            //    singletonServer.SendToClient(sr, clientID);
             

            //}
            result = true;
            return "ok";
        }
        public GetAllLogsCommand(ICollection<Log> logs,EventLog eventlogger,IReplyGenerator generator)
        {
            singletonServer = SingletonServer.Instance;
            logsClients = new List<TcpClient>();
            this.eventlogger = eventlogger;
            this.logs = logs;
            this.replyGenerator = generator;

        }
    }
}
