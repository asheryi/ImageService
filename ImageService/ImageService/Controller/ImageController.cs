using ImageService.Commands;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        private IImageServiceModal m_modal;// The Modal Object
        private Dictionary<int, ICommand> commands;
        /// <summary>
        /// ImageController constructor.
        /// </summary>
        /// <param name="modal"></param>The Modal Of The System.
        public ImageController(IImageServiceModal modal)
        {
            m_modal = modal; //Storing the Modal Of The System
            commands = new Dictionary<int, ICommand>()
            {
                { (int)CommandEnum.NewFileCommand,new NewFileCommand(modal) }
            };
        }

        /// <summary>
        /// treat the command whitch were sent to the ImageController.
        /// </summary>
        /// <param name="commandID"></param> indicates the command.
        /// <param name="args"></param> arguments need to the command.
        /// <param name="resultSuccesful"></param> indicates if the command execution succeeded or not.
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
