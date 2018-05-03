using System;

namespace ImageService.Model
{
    public class DirectoryCloseEventArgs : EventArgs
    {
        public string Message { get; set; }             // The Message That goes to the logger

        public DirectoryCloseEventArgs(string message)
        {
            
            Message = message;                          // Storing the String
        }

    }
}
