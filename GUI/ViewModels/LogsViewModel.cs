using GUI.Model;
using ImageService.Logging.Model;
using System.Collections.ObjectModel;
namespace GUI.ViewModels
{
    class LogsViewModel
    {
        private LogsModel model;
        public ObservableCollection<Log> Logs
        {
            get
            {
                return model.Logs;
            }
        }

        public LogsViewModel(LogsModel model)
        {
            this.model = model;
        }


    }
}
