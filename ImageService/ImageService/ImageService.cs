using System;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceProcess;
using System.Runtime.InteropServices;
using ImageService.Server;
using ImageService.Logging;
using System.Configuration;
using System.Collections.Generic;
using ImageService.Model;
using SharedResources.Logging;
using ImageService.Logging.Model;
using ImageService.Comunication;
using SharedResources;
//using ImageService.Logging.Model;
//using ImageService.Model;

namespace ImageService
{
    public enum ServiceState
    {
        SERVICE_STOPPED = 0x00000001,
        SERVICE_START_PENDING = 0x00000002,
        SERVICE_STOP_PENDING = 0x00000003,
        SERVICE_RUNNING = 0x00000004,
        SERVICE_CONTINUE_PENDING = 0x00000005,
        SERVICE_PAUSE_PENDING = 0x00000006,
        SERVICE_PAUSED = 0x00000007,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
        public int dwServiceType;
        public ServiceState dwCurrentState;
        public int dwControlsAccepted;
        public int dwWin32ExitCode;
        public int dwServiceSpecificExitCode;
        public int dwCheckPoint;
        public int dwWaitHint;
    };

    public partial class ImageService : ServiceBase
    {

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        private ImageServer m_imageServer;//The Image Server
        private ILoggingService logger;//the logger of the system.
        private EventLog eventLogger;//writea to logger event.
        private IContainer components;
        private Comunication.SingletonServer server;
        private ICollection<Log>  logs;//stores system logs.
        public event EventHandler<Log> LogAnnouncement;
        public ImageService()
        {
            InitializeComponent();
           
            eventLogger = new EventLog();
            string sourceName = ConfigurationManager.AppSettings["SourceName"];
            string logName = ConfigurationManager.AppSettings["LogName"];
            if (!EventLog.SourceExists(sourceName))
            {
                EventLog.CreateEventSource(
                   sourceName, logName);
            }
            eventLogger.Source = sourceName;
            eventLogger.Log = logName;
        }


       
        /// <summary>
        /// OnStart starting the Service.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {

            try
            {

                // Update the service state to Start Pending.  
                ServiceStatus serviceStatus = new ServiceStatus();
                serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
                serviceStatus.dwWaitHint = 1000;
                SetServiceStatus(this.ServiceHandle, ref serviceStatus);

                // Update the service state to Running.  
                serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
                SetServiceStatus(this.ServiceHandle, ref serviceStatus);
              
                logs = new List<Log>();

                EventLogFunc(this, new MessageRecievedEventArgs("Service is ON", MessageTypeEnum.INFO));

                

                logger = new LoggingService();
                //this function is subscribes to the event of logger.
                logger.MessageRecieved += EventLogFunc;
                EventLogFunc(this, new MessageRecievedEventArgs("MessageRecieved", MessageTypeEnum.INFO));

                server = SingletonServer.Instance;
                EventLogFunc(this, new MessageRecievedEventArgs("SingletonServer.Instance", MessageTypeEnum.INFO));

                server.Start();
                EventLogFunc(this, new MessageRecievedEventArgs("server.Start()", MessageTypeEnum.INFO));

                string manage_path = @ConfigurationManager.AppSettings["OutputDir"];
                string[] directoriesPaths = @ConfigurationManager.AppSettings["Handler"].Split(';');
                int thumbnailSize = Convert.ToInt32(ConfigurationManager.AppSettings["ThumbnailSize"]);
               
                
                logger.Logs = logs;
                m_imageServer = new ImageServer(directoriesPaths, logger,new ImageServiceModel(logger,manage_path,thumbnailSize), this,eventLogger);
                EventLogFunc(this, new MessageRecievedEventArgs(" new ImageServer", MessageTypeEnum.INFO));

            }
            catch (Exception ex)
            {
                // Log the exception.
                eventLogger.WriteEntry(ex.Message, EventLogEntryType.Error);
            }

        }
        /// <summary>
        ///  OnStop Stopping the Service.
        /// </summary>
        protected override void OnStop()
        {
            server.Stop();
            Log log = new Log(MessageTypeEnum.INFO, "Stopping Service");
        
            logs.Add(log);
            eventLogger.WriteEntry(log.Message);
            m_imageServer.terminate();
        }
        /// <summary>
        /// InitializeComponent.
        /// </summary>
        private void InitializeComponent()
        {
            this.eventLogger = new System.Diagnostics.EventLog();
            ((System.ComponentModel.ISupportInitialize)(this.eventLogger)).BeginInit();
            this.ServiceName = "ImageService";
            ((System.ComponentModel.ISupportInitialize)(this.eventLogger)).EndInit();

        }
        /// <summary>
        /// EventLogFunc writes the events to the log.
        /// </summary>
        /// <param name="sender">object that call the function.</param>
        /// <param name="args">MessageRecievedEventArgs.</param>
        private void EventLogFunc(object sender,MessageRecievedEventArgs args)
        {
             Log log = new Log(args.Type, args.Message);
            logs.Add(log);
            LogAnnouncement?.Invoke(this, log);
            eventLogger.WriteEntry(args.Message);

        }





        /// <summary>
        /// GetAllLogs returns all logs since the system started.
        /// </summary>
        //private ICollection<Log> GetAllLogs()
        //{
        //    return logs;
        //}

    }


}
