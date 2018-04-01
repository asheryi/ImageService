
using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    public class LoggingService : ILoggingService
    {
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        public void Log(string message, MessageTypeEnum type)
        {
            Directory.CreateDirectory(@"C:\Users\Brain\Documents\Visual Studio 2015\Projects\MyNewService\1\log");
            MessageRecieved?.Invoke(this, new MessageRecievedEventArgs(message,type));
        }
    }
}
