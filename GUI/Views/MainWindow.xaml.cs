using SharedResources.Logging;
using GUI.Model;
using GUI.ViewModels;
using GUI.Views.UserControls;
using ImageService.Logging.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System;
using System.Diagnostics;
using System.Windows.Data;
using System.Collections.Generic;
using SharedResources;
using System.Windows.Media;
using SharedResources.Commands;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LogsModel logsModel;
        private LogsViewModel logViewModel;
        private LogsView logsView;
        private Client client;
        private SettingsModel sm;
        private SettingsViewModel setViewModel;
        private SettingsView sv;
        public MainWindow()
        {
            

            InitializeComponent();
            //Here we Start the Tcp Connection
            //if fail return;
            //else:
            //Ask for All logs from the service assume we already got it and its call Logs
            // ObservableCollection<Log> logs=new ObservableCollection<Log>();
            ObservableCollection<Log> Logs = new ObservableCollection<Log>();
            BindingOperations.EnableCollectionSynchronization(Logs, Logs);
            logsModel = new LogsModel(Logs);
            logViewModel = new LogsViewModel(logsModel);
            logsView = new LogsView();

           
            Settings set = new Settings();
            set.LogName = "mom";
            set.OutputDirectory = "dad";
            set.SourceName = "brother";
            set.ThumbnailSize = 100;
            set.Handlers.Add(new DirectoryDetails("fggfgf"));
            set.Handlers.Add(new DirectoryDetails("sdfsdd"));
            //BindingOperations.EnableCollectionSynchronization(Logs, Logs);
            sm = new SettingsModel(set);
             setViewModel = new SettingsViewModel(sm);
             sv = new SettingsView();
            SettingsViewTab.Content = sv;
            sv.DataContext = setViewModel;
            try
            {
                //Works
                IResponseHandler handler = new ServiceReplyResponseHandler();
                client = new Client(handler);
                handler.RegisterFuncToEvent(CommandEnum.GetAllLogsCommand,recieveLogs);
                handler.RegisterFuncToEvent(CommandEnum.SendLog, recieveOneLog);
                handler.RegisterFuncToEvent(CommandEnum.GetConfigCommand, recieveSettings);

                LogsViewTab.Content = logsView;
                logsView.DataContext = logViewModel;
                client.Start();
                client.Recieve();


            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                this.Background=Brushes.Gray;
            }
            

        }

        private void recieveSettings(object sender, ContentEventArgs args)
        {

            Settings settings = args.GetContent<Settings>();
            sm.Settings.LogName = settings.LogName;
            sm.Settings.OutputDirectory = settings.OutputDirectory;
            sm.Settings.SourceName = settings.SourceName;
            sm.Settings.ThumbnailSize = settings.ThumbnailSize;
           // sm.Settings.Handlers.Clear();
           // foreach (DirectoryDetails directory in settings.Handlers)
           // {
           //     sm.Settings.Handlers.Add(directory);
           // }
           //// sm.Settings = settings;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Log log = new Log(MessageTypeEnum.FAIL, "Hey");
            logsModel.Logs.Add(log);
            ////client.SendRequest();

        }
        private void recieveLogs(object sender,ContentEventArgs args)
        {
            ICollection<Log> logs = args.GetContent<ICollection<Log>>();
          //  ICollection<Log> logs = ObjectConverter<ICollection<Log>>.Deserialize(args.Content);
            foreach (Log log in logs)
            {
                logsModel.Logs.Add(log);
            }

        }

        private void recieveOneLog(object sender, ContentEventArgs args)
        {
            logsModel.Logs.Add(args.GetContent<Log>());
          //  logsModel.Logs.Add(ObjectConverter<Log>.Deserialize(args.Content));
        }



    }
}
