using ImageService.Commands;
using ImageService.Comunication;
using ImageService.Controller.Handlers;
using ImageService.Logging;
using ImageService.Model;
using SharedResources;
using SharedResources.Commands;
using SharedResources.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        private IImageServiceModel m_Model;// The Model Object
        private Dictionary<int, ICommand> commands;
        private EventLog logger;
        private IReplyGenerator replyGenerator;
        /// <summary>
        /// ImageController constructor.
        /// </summary>
        /// <param name="Model">The Model Of The System.</param>
        public ImageController(IImageServiceModel Model,HandlersManager handlersManager,ILoggingService logger,EventLog eventLogger)
        {
            replyGenerator = new ServiceReplyGenerator();
        
            m_Model = Model; //Storing the Model Of The System
            SingletonServer singletonServer = SingletonServer.Instance;
            singletonServer.ClientConnected += ClientConnected;
    
              commands = new Dictionary<int, ICommand>()
            {
                { (int)CommandEnum.NewFileCommand,new NewFileCommand(Model) }
                , { (int)CommandEnum.CloseHandlerCommand, new CloseHandlerCommand(handlersManager)}
                , { (int)CommandEnum.SendLog, new SendLogCommand() },{ (int)CommandEnum.GetAllLogsCommand,
                      new GetAllLogsCommand(logger.Logs,eventLogger,replyGenerator) },{ (int)CommandEnum.GetConfigCommand,
                      new GetConfigCommand(replyGenerator,logger) }
            };
            this.logger = eventLogger;
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
        //Recieve log
        public void ReceiveLog(object sender, Log log)
        {
            bool result;
            string logString = ObjectConverter.Serialize(log);//Log
            ExecuteCommand((int)CommandEnum.SendLog,new string[] {logString}, out result);
        }
        public void ClientConnected(object sender,int clientID)
        {
            bool result;
            ExecuteCommand((int)CommandEnum.GetAllLogsCommand, new string[] { clientID.ToString() }, out result);

 
            ExecuteCommand((int)CommandEnum.GetConfigCommand, new string[] { clientID.ToString() }, out result);
        }
    }
}
