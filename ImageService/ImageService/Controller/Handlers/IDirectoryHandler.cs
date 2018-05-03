using ImageService.Model;
using System;

namespace ImageService.Controller.Handlers
{
    public interface IDirectoryHandler
    {
        event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed
        void StartHandleDirectory();             // The Function Recieves the directory to Handle

        // TODO DELETE THis

        //void OnCommandRecieved(object sender, CommandRecievedEventArgs e);     // The Event that will be activated upon new Command

        void Close();
    }
}
