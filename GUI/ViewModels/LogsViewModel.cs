using SharedResources.Logging;
using GUI.Model;
using ImageService.Logging.Model;
using System.Collections.ObjectModel;
using SharedResources;
using System.Collections.Generic;

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
        public void recieveLogs(object sender, ContentEventArgs args)
        {
            ICollection<Log> logs = args.GetContent<ICollection<Log>>();
            foreach (Log log in logs)
            {
                Logs.Add(log);
            }

        }

        public void recieveOneLog(object sender, ContentEventArgs args)
        {

            Logs.Add(args.GetContent<Log>());
        }

    }
}
