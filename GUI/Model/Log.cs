using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Model
{
    class Log
    {
        Log(MessageTypeEnum type, string message)
        {
            Type = type;
            Message = message;
        }
        public MessageTypeEnum Type { get; private set; }

        public string Message { get; private set; }
    }
}
