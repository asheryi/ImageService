using System;
using System.ComponentModel.DataAnnotations;
namespace WebApplication2.Models.Logging
{
    // Log information .
    public class Log : EventArgs
    {
        public Log(MessageTypeEnum type = MessageTypeEnum.INFO, string message = "")
        {
            Type = type;
            Message = message;
        }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Type")]
        public MessageTypeEnum Type { get; private set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Message")]
        public string Message { get; private set; }
        
    }
}
