using Newtonsoft.Json.Linq;
using ShaeredResources.Comunication;
using SharedResources;
using SharedResources.Commands;
using SharedResources.Communication;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Model;
using WebApplication2.Model.Communication;
using WebApplication2.Models;
using WebApplication2.Models.Logging;
using WebApplication2.Models.Models;
using WebApplication2.Models.Models.ImageWebModel;

namespace WebApplication2.Controllers
{
    public class FirstController : Controller
    {
       

      

        static Client client = new Client();
        static ImageWebModel webModel = new ImageWebModel();
        static IMessageGenerator messGenerator = new CommunicationMessageGenerator();
        static SettingsModel settingsModel = new SettingsModel();
        static LogsModel logsModel = new LogsModel();
        static IMessageHandler messHandler; // MAYBE NOT
        static PhotosModel photosModel;
        static AutoResetEvent resultReset;
        public FirstController()
        {
            if (!client.Connected)
            {
                resultReset = new AutoResetEvent(false);
                settingsModel.HandlerRemoveEvent = null;
                settingsModel.SettingsRecievedEvent = null;
                logsModel.recieveLogsEvent=null;

                settingsModel.HandlerRemoveEvent += HandlerRemoved;
                settingsModel.SettingsRecievedEvent += SettingsRecieved;
                logsModel.recieveLogsEvent += LogsRecieved;


                messHandler = new CommunicationMessageHandler();

                messHandler.RegisterFuncToEvent(CommandEnum.GetAllLogsCommand, logsModel.recieveLogs);
                messHandler.RegisterFuncToEvent(CommandEnum.SendLog, logsModel.recieveOneLog);

                messHandler.RegisterFuncToEvent(CommandEnum.GetConfigCommand, settingsModel.recieveSettings);
                messHandler.RegisterFuncToEvent(CommandEnum.CloseHandlerCommand, settingsModel.removeHandler);

              
                client.messageHandler = messHandler;
                bool connected = client.Start(); ;
               
                client.Recieve();
                if (connected)
                {
                    resultReset.WaitOne();
                    resultReset.WaitOne();
                    photosModel = new PhotosModel(settingsModel.Settings.OutputDirectory, settingsModel.Settings.ThumbnailSize);
                    messHandler.RegisterFuncToEvent(CommandEnum.NewFileCommand, photosModel.newFileRecieved);
                }
            }
        }

        
        // GET: First
        public ActionResult Config()
        {
            if (!client.Connected)
            {

                return RedirectToAction("ImageWeb");
            }
            return View(settingsModel.Settings);
        }
        public ActionResult ImageWeb()
        {

            ViewBag.Connected = client.Connected;
            webModel.OutputPath = settingsModel.Settings.OutputDirectory;
            ViewBag.PhotosCount = webModel.GetPhotosCount();
            return View();
        }
       


        
        public ActionResult DeleteImage(string path,string name,string descreption,string thumbnailPath)
        {

            Photo photo=new Photo();
            photo.Path = path;
            photo.Name = name;
            photo.ThumbnailPath = thumbnailPath;
            photo.Descreption = descreption;
            ViewBag.thumnailSize = photosModel.ThumbnailSize;
            return View(photo);
        }
        public ActionResult DeleteImageRequest(string path)
        {
            photosModel.DeleteImage(path);
            return RedirectToAction("Photos");
        }
        // GET: First/Details
        public ActionResult Logs()
        {
            if (!client.Connected)
            {
                return RedirectToAction("ImageWeb");
            }
            return View(logsModel.Logs);
            
        }
       
        public void getAllLogs()
        {
            client.Send(messGenerator.Generate(CommandEnum.GetAllLogsCommand,""));
        }

        public ActionResult Photos()
        {
            if (!client.Connected)
            {
                return RedirectToAction("ImageWeb");
            }
            return View(photosModel);
        }
        

        public ActionResult RemoveHandler(string handler)
        {
            
            ViewBag.handlerToRemove = handler;
            return View();
        }
        public void LogsRecieved()
        {
            resultReset.Set();
        }
        static private DirectoryDetails selecteDirectoryDetails;
        [HttpPost]
        public void RemoveHandlerRequest(string handler)
        {
            selecteDirectoryDetails = new DirectoryDetails(handler);
            
            string send = messGenerator.Generate(CommandEnum.CloseHandlerCommand, selecteDirectoryDetails);
           
            client.Send(send);
            //waiting to the handler to be delete
            resultReset.WaitOne();
           
            
        }
        public void HandlerRemoved(object obj,DirectoryDetails handler)
        {
            if (handler.DirectoryName == selecteDirectoryDetails.DirectoryName)
            {
                resultReset.Set();
            }
        }
        public void SettingsRecieved(object obj,EventArgs e)
        {
            resultReset.Set();
        }
        
       
        public ActionResult Image(string path,string name,string date,string descreption,string thumbnailPath)
        {
           
            Photo photo = new Photo();
            photo.Path = path;
            photo.Name = name;
            photo.Descreption = descreption;
            photo.ThumbnailPath = thumbnailPath;
            DateTime dateTime;
            DateTime.TryParse(date, out dateTime);
            photo.Date = dateTime;
            return View(photo);
        }
        
       
    }
}
