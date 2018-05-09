﻿
using SharedResources.Logging;
using ImageService.Logging.Model;
using System;
using System.Collections.Generic;

namespace ImageService.Logging
{
    public class LoggingService : ILoggingService
    {
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        private ICollection<Log> logs;
        public void Log(string message, MessageTypeEnum type)
        {
            MessageRecieved?.Invoke(this, new MessageRecievedEventArgs(message,type));
        }
        public ICollection<Log> Logs
        {
            set
            {
                logs = value;
            }
            get
            {
                return logs;
            }
        }
    }
}
