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
            try
            {
                IResponseHandler handler = new ServiceReplyResponseHandler();
                client = new Client(handler);
                handler.RegisterFuncToEvent(CommandEnum.GetAllLogsCommand,recieveLogs);
                handler.RegisterFuncToEvent(CommandEnum.SendLog, recieveOneLog);

                client.Recieve();
                LogsViewTab.Content = logsView;
                logsView.DataContext = logViewModel;

               
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
                this.Background=Brushes.Gray;
            }
            

        }

    

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Log log = new Log(MessageTypeEnum.FAIL, "Hey");
            logsModel.Logs.Add(log);
            ////client.SendRequest();

        }
        private void recieveLogs(object sender,ContentEventArgs args)
        {
            ICollection<Log> logs = args.geContent<ICollection<Log>>();
          //  ICollection<Log> logs = ObjectConverter<ICollection<Log>>.Deserialize(args.Content);
            foreach (Log log in logs)
            {
                logsModel.Logs.Add(log);
            }

        }

        private void recieveOneLog(object sender, ContentEventArgs args)
        {
            logsModel.Logs.Add(args.geContent<Log>());
          //  logsModel.Logs.Add(ObjectConverter<Log>.Deserialize(args.Content));
        }



    }
}
