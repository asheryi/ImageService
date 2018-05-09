using Newtonsoft.Json;
using SharedResources.Commands;
using System;

namespace SharedResources.Communication
{
    [Serializable]
    public class ServiceReply
    {
        public CommandEnum CommandID { get; private set; }
        public string Content { get; private set; }

        public ServiceReply(CommandEnum commandID, string content)
        {
            CommandID = commandID;
            Content = content;
        }
    }
}
