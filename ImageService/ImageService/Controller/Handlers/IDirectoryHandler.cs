using ImageService.Model;
using System;

namespace ImageService.Controller.Handlers
{
    public interface IDirectoryHandler
    {
        event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed
        void StartHandleDirectory();             // The Function Recieves the directory to Handle

       

        void Close();
    }
}
