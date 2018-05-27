using System.ComponentModel;
using System.Collections.ObjectModel;
using ImageService.Logging.Model;
using SharedResources.Logging;
using System.Windows.Data;

namespace GUI.Model
{
    class LogsModel
    {
       public event PropertyChangedEventHandler PropertyChanged;
      
        private ObservableCollection<Log> logs;//Contains the log sent by the server.
       /// <summary>
       /// Logs collection property.
       /// </summary>
        public ObservableCollection<Log> Logs
        {
            get
            {
                return logs;
            }
            private set
            {
                logs = value;
               
            }
        }
        /// <summary>
        /// LogsModel constructor.
        /// </summary>
        public LogsModel()
        {
            this.Logs = new ObservableCollection<Log>();
            //Allows more than one thread access the collection.
            BindingOperations.EnableCollectionSynchronization(Logs, Logs);

        }


    }
}
