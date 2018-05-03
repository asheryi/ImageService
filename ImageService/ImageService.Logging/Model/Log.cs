namespace ImageService.Logging.Model
{
    public class Log
    {
       public Log(MessageTypeEnum type, string message)
        {
            Type = type;
            Message = message;
        }
        public MessageTypeEnum Type { get; private set; }

        public string Message { get; private set; }
    }
}
