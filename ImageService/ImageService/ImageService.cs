using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ImageService.Server;
using ImageService.Controller;
using ImageService.Modal;
using ImageService.Logging;
using System.Threading;
using ImageService.Logging.Modal;
using System.Configuration;
using ImageService.Infrastructure;

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
        private EventLog eventLogger;//write to logger event.
        private IContainer components;

    
        public ImageService()
        {
            InitializeComponent();

            eventLogger = new System.Diagnostics.EventLog();
            string sourceName = ConfigurationManager.AppSettings["SourceName"];
            string logName = ConfigurationManager.AppSettings["LogName"];
            if (!System.Diagnostics.EventLog.SourceExists(sourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(
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

                eventLogger.WriteEntry("Service is ON");

                logger = new LoggingService();
                //this function is subscribes to the event of logger.
                logger.MessageRecieved += EventLogFunc;



                string manage_path = @ConfigurationManager.AppSettings["OutputDir"];

                m_imageServer = new ImageServer(@ConfigurationManager.AppSettings["Handler"].Split(';'), logger,new ImageServiceModal(logger,manage_path,Convert.ToInt32(ConfigurationManager.AppSettings["ThumbnailSize"])));

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
            eventLogger.WriteEntry("Stopping Service");
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
        /// <param name="sender"></param>object that call the function.
        /// <param name="args"></param>MessageRecievedEventArgs.
        private void EventLogFunc(object sender,MessageRecievedEventArgs args)
        {
            eventLogger.WriteEntry(args.Message);
        }
    }


}
