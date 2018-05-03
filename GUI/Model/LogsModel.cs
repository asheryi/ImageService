using System.ComponentModel;
using System.Collections.ObjectModel;
using ImageService.Logging.Model;

namespace GUI.Model
{
    class LogsModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Log> logs;
        public ObservableCollection<Log> Logs
        {
            get
            {
                return logs;
            }
            private set
            {
                logs = value;
                OnPropertyChanged("Logs");
            }
        }

        public LogsModel(ObservableCollection<Log> prev_logs)
        {
            this.Logs = prev_logs;

        }
        public LogsModel()
        {
            this.Logs = new ObservableCollection<Log>();
        }



        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
         
    }
}
