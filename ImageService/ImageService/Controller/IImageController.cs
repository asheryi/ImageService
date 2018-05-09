using SharedResources.Logging;

namespace ImageService.Controller
{
    public interface IImageController
    {
        string ExecuteCommand(int commandID, string[] args, out bool result);          // Executing the Command Requet
                                                                                       //Recieve log
        void ReceiveLog(object sender, Log log);
    }
}
