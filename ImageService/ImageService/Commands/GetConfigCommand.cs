using ImageService.Comunication;
using ImageService.Logging;
using SharedResources;
using SharedResources.Commands;
using SharedResources.Communication;
using System;
using System.Configuration;
using System.Linq;

namespace ImageService.Commands
{
    public class GetConfigCommand : ICommand
    {
        private SingletonServer singletonServer;
        private IReplyGenerator replyGenerator;
        private Settings settings;
        private ILoggingService logger;
        public GetConfigCommand(IReplyGenerator replyGenerator,ILoggingService logger)
        {
            singletonServer = SingletonServer.Instance;
            this.replyGenerator = replyGenerator;
            string outputDirectory = @ConfigurationManager.AppSettings["OutputDir"];
            string sourceName = @ConfigurationManager.AppSettings["SourceName"];
            string logName = @ConfigurationManager.AppSettings["LogName"];
            int thumbnailSize = Convert.ToInt32(ConfigurationManager.AppSettings["ThumbnailSize"]);
            settings = new Settings(outputDirectory, sourceName, logName, thumbnailSize);
            this.logger = logger;

        }
        public string Execute(string[] args, out bool result)
        {
            int clientID = Int32.Parse(args[0]);
            result = true;

            //logger.Log("ITS GOING", SharedResources.Logging.MessageTypeEnum.FAIL);
            // WASTE ? DELETE ?
            string[] directoriesPaths = @ConfigurationManager.AppSettings["Handler"].Split(';');
            foreach (string directory in directoriesPaths)
            {
                settings.Handlers.Add(new DirectoryDetails(directory));
            }
           // string s = this.replyGenerator.Generate(CommandEnum.GetConfigCommand, settings);
            //Settings set = ObjectConverter.Deserialize<Settings>(ObjectConverter.Deserialize<ServiceReply>(s).Content);
            //logger.Log(set.Handlers[0].DirectoryName, SharedResources.Logging.MessageTypeEnum.FAIL);
            singletonServer.SendToClient(this.replyGenerator.Generate(CommandEnum.GetConfigCommand, settings), clientID);

            logger.Log("ITS AFTER"+ CommandEnum.GetConfigCommand.ToString(), SharedResources.Logging.MessageTypeEnum.WARNING);

            return "ok";
        }
       
        public void deleteHandler(string directory)
        {
            var itemToRemove = settings.Handlers.Single(r => r.DirectoryName == directory);
            settings.Handlers.Remove(itemToRemove);
        }
    }
}
