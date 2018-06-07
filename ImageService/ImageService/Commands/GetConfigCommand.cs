using ImageService.Comunication;
using ShaeredResources.Comunication;
using SharedResources;
using SharedResources.Commands;
using SharedResources.Communication;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace ImageService.Commands
{
    public class GetConfigCommand : ICommand
    {
        private SingletonServer singletonServer;
        private IMessageGenerator messageGenerator;
        private Settings settings;
        private Func<ICollection<string>> getHandlersPaths;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageGenerator">The generator of communication messages</param>
        /// <param name="getHandlersPaths">delegate to function to return the handlers details</param>
        public GetConfigCommand(IMessageGenerator messageGenerator,Func<ICollection<string>> getHandlersPaths)
        {
            singletonServer = SingletonServer.Instance;
            this.messageGenerator = messageGenerator;
            string outputDirectory = @ConfigurationManager.AppSettings["OutputDir"];
            string sourceName = @ConfigurationManager.AppSettings["SourceName"];
            string logName = @ConfigurationManager.AppSettings["LogName"];
            int thumbnailSize = Convert.ToInt32(ConfigurationManager.AppSettings["ThumbnailSize"]);
            settings = new Settings(outputDirectory, sourceName, logName, thumbnailSize);
            
            this.getHandlersPaths = getHandlersPaths;

        }
        public string Execute(string[] args, out bool result)
        {
            result = true;
            // Gets the paths
            ICollection<string> directoriesPaths = getHandlersPaths();
            settings.Handlers.Clear(); // CLears
            // Adds them to the settings' handlers
            foreach (string directory in directoriesPaths)
            {
                settings.Handlers.Add(new DirectoryDetails(directory));
            }
            // send generated message based on the message generator.
            string send = this.messageGenerator.Generate(CommandEnum.GetConfigCommand, settings);
            singletonServer.SendToClient(send, new ClientID(args));

            return "ok";
        }
    }
}
