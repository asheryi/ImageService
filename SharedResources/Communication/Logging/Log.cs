using System;

namespace SharedResources.Logging
{
    [Serializable]
    public class Log : EventArgs
    {
       //private DateTime recieveDate;
        public Log(MessageTypeEnum type = MessageTypeEnum.INFO, string message = "")
        {
            Type = type;
            Message = message;
           //recieveDate = DateTime.Now;
        }
        public MessageTypeEnum Type { get; private set; }

        public string Message { get; private set; }
        //public DateTime recieveAt()
        //{
        //    return recieveDate;
        //}
    }
}
