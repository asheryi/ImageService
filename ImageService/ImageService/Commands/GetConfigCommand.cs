using ImageService.Comunication;
using ImageService.Controller.Handlers;
using ImageService.Logging;
using SharedResources;
using SharedResources.Commands;
using SharedResources.Communication;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace ImageService.Commands
{
    public class GetConfigCommand : ICommand
    {
        private SingletonServer singletonServer;
        private IMessageGenerator replyGenerator;
        private Settings settings;
        private ILoggingService logger;
        private Func<ICollection<string>> getHandlersPaths;
        public GetConfigCommand(IMessageGenerator replyGenerator,ILoggingService logger,Func<ICollection<string>> getHandlersPaths)
        {
            singletonServer = SingletonServer.Instance;
            this.replyGenerator = replyGenerator;
            string outputDirectory = @ConfigurationManager.AppSettings["OutputDir"];
            string sourceName = @ConfigurationManager.AppSettings["SourceName"];
            string logName = @ConfigurationManager.AppSettings["LogName"];
            int thumbnailSize = Convert.ToInt32(ConfigurationManager.AppSettings["ThumbnailSize"]);
            settings = new Settings(outputDirectory, sourceName, logName, thumbnailSize);
            this.logger = logger;
           
            this.getHandlersPaths = getHandlersPaths;

        }
        public string Execute(string[] args, out bool result)
        {
            result = true;
            ICollection<string> directoriesPaths = getHandlersPaths();
            settings.Handlers.Clear();
            foreach (string directory in directoriesPaths)
            {
                settings.Handlers.Add(new DirectoryDetails(directory));
            }
            string send = this.replyGenerator.Generate(CommandEnum.GetConfigCommand, settings);

            //IPAddress address = IPAddress.Parse(args[0]);
            //int port = int.Parse(args[1]);
            //the client's IPEndPoint who need to get the massage
            //IPEndPoint p = new IPEndPoint(address, port);
            singletonServer.SendToClient(send, new ClientID(args));
            return "ok";
        }
       
        public void deleteHandler(string directory)
        {
            var itemToRemove = settings.Handlers.Single(r => r.DirectoryName == directory);
            settings.Handlers.Remove(itemToRemove);
        }
    }
}
