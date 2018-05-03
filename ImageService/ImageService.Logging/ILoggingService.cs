using ImageService.Logging.Model;
using System;

namespace ImageService.Logging
{
    public interface ILoggingService
    {
        event EventHandler<MessageRecievedEventArgs> MessageRecieved; // The event to register to recieve messages
        void Log(string message, MessageTypeEnum type); // Logging the Message
    }
}
