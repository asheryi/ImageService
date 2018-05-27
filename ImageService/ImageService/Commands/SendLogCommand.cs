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
    // Send specific log to all !
    class SendLogCommand:ICommand
    {
        SingletonServer singletonServer;
        private IMessageGenerator messageGenerator;
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="messageGenerator">generator of messages of communication</param>
        public SendLogCommand(IMessageGenerator messageGenerator)
        {
            singletonServer = SingletonServer.Instance;
            this.messageGenerator = messageGenerator;

        }
        public string Execute(string[] args, out bool result)
        {
            // Args[0] is message type , Args[1] is the message
            singletonServer.SendToAll(messageGenerator.Generate(CommandEnum.SendLog,new Log((MessageTypeEnum)(int.Parse(args[0])),args[1])));
            result = true;
            return "ok";
        }
    }
}
