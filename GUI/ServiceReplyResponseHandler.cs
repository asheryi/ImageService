using SharedResources;
using SharedResources.Commands;
using SharedResources.Communication;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GUI
{
    public class ServiceReplyResponseHandler : IResponseHandler
    {
        private IDictionary<CommandEnum, EventHandler<ContentEventArgs>> eventHandlerDic;

        //private IDictionary<CommandEnum, Type> commandToType;

        public ServiceReplyResponseHandler()
        {
            eventHandlerDic = new Dictionary<CommandEnum, EventHandler<ContentEventArgs>>();
            eventHandlerDic.Add(CommandEnum.GetAllLogsCommand, null);
            eventHandlerDic.Add(CommandEnum.GetConfigCommand, null);
            eventHandlerDic.Add(CommandEnum.SendLog, null);
        }

        public bool Handle(string raw_data)
        {
            new Task(() =>
            {
                try
                {
                    ServiceReply reply = ObjectConverter.Deserialize<ServiceReply>(raw_data);//<ServiceReply>
                    EventHandler<ContentEventArgs> eventhandler = eventHandlerDic[reply.CommandID];
                    eventhandler?.Invoke(this, new ContentEventArgs(reply.Content));
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.StackTrace);
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
