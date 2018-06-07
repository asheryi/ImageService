using Newtonsoft.Json;
using SharedResources.Commands;
using System;

namespace SharedResources.Communication
{
    [Serializable]
    // Specific protocol of messages transferring .
    public class CommunicationMessage
    {
        public CommandEnum CommandID { get; private set; } // the command
        public string Content { get; private set; } // the details of message (a serialized object)

        public CommunicationMessage(CommandEnum commandID, string content)
        {
            CommandID = commandID;
            Content = content;
        }
    }
}
