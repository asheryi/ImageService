using System;

namespace ImageService.Logging.Modal
{
    public class MessageRecievedEventArgs : EventArgs
    {
        public MessageTypeEnum Status { get; set; }
        public string Message { get; set; }

        public MessageRecievedEventArgs(string message = "",MessageTypeEnum status = MessageTypeEnum.INFO)
        {
            Status = status;
            Message = message;
        }
    }
}
