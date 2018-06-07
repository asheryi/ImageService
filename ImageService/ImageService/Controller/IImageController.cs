using SharedResources.Logging;

namespace ImageService.Controller
{
    public interface IImageController
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="commandID">Indicates the command type</param>
        /// <param name="args">Args for the command.</param>
        /// <param name="result">Indicates if the execution succeeded</param>
        /// <returns></returns>
        string ExecuteCommand(int commandID, string[] args, out bool result);          // Executing the Command Requet
                                               
        /// <summary>
        /// Receive Log from the logging.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="log"></param>
        void ReceiveLog(object sender, Log log);
    }
}
