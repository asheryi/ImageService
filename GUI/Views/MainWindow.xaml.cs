using GUI.Model;
using GUI.ViewModels;
using GUI.Views.UserControls;
using ImageService.Logging.Model;
using System.Windows;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LogsModel logsModel;
        LogsViewModel logViewModel;
        LogsView logsView;
        public MainWindow()
        {
            InitializeComponent();
            //Here we Start the Tcp Connection
            //if fail return;
            //else:
            //Ask for All logs from the service assume we already got it and its call Logs
           // ObservableCollection<Log> logs=new ObservableCollection<Log>();
           
              logsModel = new LogsModel(new System.Collections.ObjectModel.ObservableCollection<Log>() { new Log(MessageTypeEnum.WARNING,"I I I I "), new Log(MessageTypeEnum.INFO, "fmdljcudI ") });
             logViewModel = new LogsViewModel(logsModel);
             logsView = new LogsView();
            LogsViewTab.Content = logsView;
            // logsView.DataContext = logViewModel.Logs;
            logsView.DataContext = logViewModel;


        }

    

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Log log = new Log(MessageTypeEnum.FAIL, "Hey");
            logsModel.Logs.Add(log);
        }
    }
}
