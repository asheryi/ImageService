using ImageService.Comunication;
using SharedResources;
using SharedResources.Commands;
using SharedResources.Communication;
using SharedResources.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class SendLogCommand:ICommand
    {
        SingletonServer singletonServer;
        private ICollection<TcpClient> logsClients;
        private IMessageGenerator replyGenerator;
      public  SendLogCommand(IMessageGenerator replyGenerator)
        {
            singletonServer = SingletonServer.Instance;
            logsClients = new List<TcpClient>();
            this.replyGenerator = replyGenerator;

        }
        public string Execute(string[] args, out bool result)
        {
            singletonServer.SendToAll(replyGenerator.Generate(CommandEnum.SendLog,new Log((MessageTypeEnum)(int.Parse(args[0])),args[1])));
            //singletonServer.SendToAll(new CommunicationMessage(CommandEnum.SendLog, args[0]));
            result = true;
            return "ok";
        }
    }
}
