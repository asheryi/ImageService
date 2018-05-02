using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

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
                Logs = value;
                OnPropertyChanged("Logs");
            }
        }

        public LogsModel(ObservableCollection<Log> prev_logs)
        {
            this.Logs = prev_logs;


        }



        
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
         
    }
}
