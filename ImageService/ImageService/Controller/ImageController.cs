﻿using ImageService.Commands;
using ImageService.Comunication;
using ImageService.Controller.Handlers;
using ImageService.Logging;
using ImageService.Model;
using ShaeredResources.Comunication;
using SharedResources;
using SharedResources.Commands;
using SharedResources.Communication;
using SharedResources.Logging;
using System;
using System.Collections.Generic;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
       
        private Dictionary<int, ICommand> commands;
        private IMessageGenerator messageGenerator;
        private ILoggingService loggingService;
        private HandlersManager handlersManager;

        /// <summary>
        /// ImageController constructor.
        /// </summary>
        /// <param name="Model">The Model Of The System.</param>
        public ImageController(ImageControllerArgs imageControllerArgs,ref EventHandler<Log> logAnoun,ref Action serverDown)
        {
            imageControllerArgs.ImageServiceModelArgs.LoggingService = imageControllerArgs.LoggingService;
            loggingService = imageControllerArgs.LoggingService;
            //imageControllerArgs.LogAnnouncement += ReceiveLog;
            //logAnoun += ReceiveLog;
            messageGenerator = new CommunicationMessageGenerator();
           
            // m_Model = imageControllerArgs.ImageServiceModel; //Storing the Model Of The System
            SingletonServer singletonServer = SingletonServer.Instance;
            CommunicationMessageHandler CommunicationMessageMessageHandler = new CommunicationMessageHandler();
            singletonServer.MessageHandler = CommunicationMessageMessageHandler;
            //CommunicationMessageMessageHandler.RegisterFuncToEvent(CommandEnum.CloseHandlerCommand, closeHandler);
            //CommunicationMessageMessageHandler.RegisterFuncToEvent(CommandEnum.GetAllLogsCommand, getAllCommand);

            singletonServer.Start();


            singletonServer.ClientConnected += ClientConnectedAndroid;
            singletonServer.BitmapTransfer += ImageTransfer;
            handlersManager = new HandlersManager(this, imageControllerArgs.DirectoriesPaths, loggingService, imageControllerArgs.HandlerDirectoryClose,ref serverDown);
           
            commands = new Dictionary<int, ICommand>()
            {
                { (int)CommandEnum.NewFileCommand,new NewFileCommand(new ImageServiceModel(imageControllerArgs.ImageServiceModelArgs)) }
                , { (int)CommandEnum.CloseHandlerCommand, new CloseHandlerCommand(handlersManager)}
                , { (int)CommandEnum.SendLog, new SendLogCommand(messageGenerator) },{ (int)CommandEnum.GetAllLogsCommand,
                      new GetAllLogsCommand(loggingService.Logs,messageGenerator) },{ (int)CommandEnum.GetConfigCommand,
                      new GetConfigCommand(messageGenerator,handlersManager.getHandlersPaths) }
            };
        }

        private void ClientConnectedAndroid(object sender, ClientID e)
        {
            loggingService.Log("Client connected", MessageTypeEnum.INFO);
        }

        private void ImageTransfer(object sender, Bitmap bitmap)
        {

            string watchFolder = (handlersManager.getHandlersPaths()[0]);
            
    
            string path = watchFolder + @"\" + bitmap.Name;
            try
            {

                
                bitmap.Image.Save(path);

               
            }
            catch(Exception e)
            {

            }
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
            ExecuteCommand((int)CommandEnum.SendLog,new string[] {((int)log.Type).ToString(), log.Message }, out result);
        }
        public void ClientConnected(object sender, ClientID clientID)
        {
            loggingService.Log("Client connected",MessageTypeEnum.INFO);
            bool result;
            try
            {
                ExecuteCommand((int)CommandEnum.GetAllLogsCommand, clientID.getArgs(), out result);
            } catch(Exception e)
            {
                loggingService.Log(e.Message,MessageTypeEnum.FAIL);
            }
            ExecuteCommand((int)CommandEnum.GetConfigCommand, clientID.getArgs(), out result);
        }
        public void closeHandler(object sender,ContentEventArgs args)
        {
            bool result;
            ExecuteCommand((int)CommandEnum.CloseHandlerCommand,new string[] { args.GetContent<DirectoryDetails>().DirectoryName }, out result);
           
        }
        public void getAllCommand(object sender, ContentEventArgs args)
        {
            bool result;
            
            loggingService.Log("web wants all logs "+args.ClientID.getArgs()[0], MessageTypeEnum.INFO);
            ExecuteCommand((int)CommandEnum.GetAllLogsCommand, args.ClientID.getArgs(), out result);
        }
    }
}
