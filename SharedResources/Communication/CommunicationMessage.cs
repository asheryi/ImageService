using Newtonsoft.Json;
using SharedResources.Commands;
using System;

namespace SharedResources.Communication
{
    [Serializable]
    public class CommunicationMessage
    {
        public CommandEnum CommandID { get; private set; }
        public string Content { get; private set; }

        public CommunicationMessage(CommandEnum commandID, string content)
        {
            CommandID = commandID;
            Content = content;
        }
    }
}
