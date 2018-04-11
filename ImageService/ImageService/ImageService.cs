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

        private ImageServer m_imageServer;          // The Image Server
        private ILoggingService logging;
        private EventLog eventLogger;
        private IContainer components;


        public ImageService()
        {
            InitializeComponent();

            eventLogger = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("MyImageServiceSource"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "MyImageServiceSource", "MyImageServiceLog");
            }
            eventLogger.Source = "MyImageServiceSource";
            eventLogger.Log = "MyImageServiceLog";
        }


        // Here You will Use the App Config!
        protected override void OnStart(string[] args)
        {
            try
            {
                eventLogger.WriteEntry("START PENDING");


                // Update the service state to Start Pending.  
                ServiceStatus serviceStatus = new ServiceStatus();
                serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
                serviceStatus.dwWaitHint = 1000;
                SetServiceStatus(this.ServiceHandle, ref serviceStatus);


                // TODO fill
                

                // Update the service state to Running.  
                serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
                SetServiceStatus(this.ServiceHandle, ref serviceStatus);

                eventLogger.WriteEntry("Now ITS ON!!");

                logging = new LoggingService();
                logging.MessageRecieved += EventLogFunc;

                eventLogger.WriteEntry("BEFORE CREATE SERVER");
                string manage_path = @"C:\Users\1\Desktop\manage";

                m_imageServer = new ImageServer(new string[] { @"C:\Users\1\Desktop\watch", @"C:\Users\1\Desktop\watch2" }, logging,new ImageServiceModal(logging,manage_path,120));
                eventLogger.WriteEntry("AFTER CREATE SERVER");

            }
            catch (Exception ex)
            {
                // Log the exception.
                eventLogger.WriteEntry(ex.Message, EventLogEntryType.Error);
            }

        }

        protected override void OnStop()
        {
            eventLogger.WriteEntry("in onStop");
            m_imageServer.terminate();
        }

        private void InitializeComponent()
        {
            this.eventLogger = new System.Diagnostics.EventLog();
            ((System.ComponentModel.ISupportInitialize)(this.eventLogger)).BeginInit();


            this.ServiceName = "ImageService";

            ((System.ComponentModel.ISupportInitialize)(this.eventLogger)).EndInit();

        }

        private void EventLogFunc(object sender,MessageRecievedEventArgs args)
        {
            eventLogger.WriteEntry(args.Message);
        }
    }


}
