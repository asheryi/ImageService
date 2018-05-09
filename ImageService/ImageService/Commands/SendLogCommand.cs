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

      public  SendLogCommand()
        {
            singletonServer = SingletonServer.Instance;
            logsClients = new List<TcpClient>();

        }
        public string Execute(string[] args, out bool result)
        {
            singletonServer.SendToAll(new ServiceReply(CommandEnum.SendLog,args[0]));
            result = true;
            return "ok";
        }
    }
}
