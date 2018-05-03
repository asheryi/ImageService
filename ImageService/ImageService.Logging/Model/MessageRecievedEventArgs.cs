using System;

namespace ImageService.Logging.Model
{
    public class MessageRecievedEventArgs : EventArgs
    {
        public MessageTypeEnum Type { get; set; }
        public string Message { get; set; }

        public MessageRecievedEventArgs(string message = "",MessageTypeEnum type = MessageTypeEnum.INFO)
        {
            Type = type;
            Message = message;
        }
    }
}
