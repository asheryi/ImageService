using SharedResources;
using SharedResources.Commands;
using SharedResources.Communication;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SharedResources.Communication
{
    // Handler of the communication messages (specific protocol) 
    public class CommunicationMessageHandler : IMessageHandler
    {
        private IDictionary<CommandEnum, EventHandler<ContentEventArgs>> eventHandlerDictionary;

        public CommunicationMessageHandler()
        {
            // initialize
            eventHandlerDictionary = new Dictionary<CommandEnum, EventHandler<ContentEventArgs>>();
            eventHandlerDictionary.Add(CommandEnum.GetAllLogsCommand, null);
            eventHandlerDictionary.Add(CommandEnum.GetConfigCommand, null);
            eventHandlerDictionary.Add(CommandEnum.SendLog, null);
            eventHandlerDictionary.Add(CommandEnum.CloseHandlerCommand, null);
        }

        /// <summary>
        /// Handles in a new task (paralel) the data from the connection ,
        /// desrialize it , then according to the command inviokes the specific
        /// event (by the dictionary).
        /// </summary>
        /// <param name="raw_data">data from the connection</param>
        /// <returns></returns>
        public bool Handle(string raw_data)
        {
            new Task(() =>
            {
                try
                {
                    CommunicationMessage reply = ObjectConverter.Deserialize<CommunicationMessage>(raw_data);//<CommunicationMessage>
                    EventHandler<ContentEventArgs> eventhandler = eventHandlerDictionary[reply.CommandID];
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
            if (!eventHandlerDictionary.Keys.Contains(c))
            {
                return false;
            }
            eventHandlerDictionary[c] += func;
            return true;

        }
    }
}
