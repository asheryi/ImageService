using ImageService.Commands;
using ImageService.Infrastructure.Enums;
using ImageService.Model;
using System.Collections.Generic;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        private IImageServiceModel m_Model;// The Model Object
        private Dictionary<int, ICommand> commands;
        /// <summary>
        /// ImageController constructor.
        /// </summary>
        /// <param name="Model">The Model Of The System.</param>
        public ImageController(IImageServiceModel Model)
        {
            m_Model = Model; //Storing the Model Of The System
            commands = new Dictionary<int, ICommand>()
            {
                { (int)CommandEnum.NewFileCommand,new NewFileCommand(Model) }
            };
        }

        /// <summary>
        /// treat the command whitch were sent to the ImageController.
        /// </summary>
        /// <param name="commandID">indicates the command.</param> 
        /// <param name="args">arguments need to the command.</param> 
        /// <param name="resultSuccesful">indicates if the command execution succeeded or not.</param>
        /// <returns></returns>
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            if (!commands.ContainsKey(commandID))
            {
                resultSuccesful = false; 
                return "";
            }

            return commands[commandID].Execute(args, out resultSuccesful);
        }
    }
}
