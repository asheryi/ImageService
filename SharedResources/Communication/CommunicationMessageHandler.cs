using SharedResources;
using SharedResources.Commands;
using SharedResources.Communication;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SharedResources.Communication
{
    public class CommunicationMessageHandler : IMessageHandler
    {
        private IDictionary<CommandEnum, EventHandler<ContentEventArgs>> eventHandlerDic;

        //private IDictionary<CommandEnum, Type> commandToType;

        public CommunicationMessageHandler()
        {
            eventHandlerDic = new Dictionary<CommandEnum, EventHandler<ContentEventArgs>>();
            eventHandlerDic.Add(CommandEnum.GetAllLogsCommand, null);
            eventHandlerDic.Add(CommandEnum.GetConfigCommand, null);
            eventHandlerDic.Add(CommandEnum.SendLog, null);
            eventHandlerDic.Add(CommandEnum.CloseHandlerCommand, null);
        }

        public bool Handle(string raw_data)
        {
            new Task(() =>
            {
                try
                {
                    CommunicationMessage reply = ObjectConverter.Deserialize<CommunicationMessage>(raw_data);//<CommunicationMessage>
                    EventHandler<ContentEventArgs> eventhandler = eventHandlerDic[reply.CommandID];
                    eventhandler?.Invoke(this, new ContentEventArgs(reply.Content));
                }
                catch (Exception)
                {
                   
                }

            }).Start();
            return true;
        }

        public bool RegisterFuncToEvent(CommandEnum c, EventHandler<ContentEventArgs> func)
        {
            if (!eventHandlerDic.Keys.Contains(c))
            {
                return false;
            }
            eventHandlerDic[c] += func;
            return true;

        }
    }
}
