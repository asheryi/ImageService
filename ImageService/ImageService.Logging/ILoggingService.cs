using SharedResources.Logging;
using ImageService.Logging.Model;
using System;
using System.Collections.Generic;

namespace ImageService.Logging
{
    public interface ILoggingService
    {
        event EventHandler<MessageRecievedEventArgs> MessageRecieved; // The event to register to recieve messages
        void Log(string message, MessageTypeEnum type); // Logging the Message
        ICollection<Log> Logs
        {
            get;
            set;
        }
    }
}
