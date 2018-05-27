using System;

namespace SharedResources.Logging
{
    // Log information .
    public class Log : EventArgs
    {
        public Log(MessageTypeEnum type = MessageTypeEnum.INFO, string message = "")
        {
            Type = type;
            Message = message;
        }
        public MessageTypeEnum Type { get; private set; }

        public string Message { get; private set; }
        
    }
}
