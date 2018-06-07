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
        
        private LogsModel model;//logsModel.
        /// <summary>
        /// Logs collection property.
        /// </summary>
        public ObservableCollection<Log> Logs
        {
            get
            {
                return model.Logs;
            }
        }
        /// <summary>
        /// LogViewModel constructor.
        /// </summary>
        /// <param name="model">logsModel</param>
        public LogsViewModel(LogsModel model)
        {
            this.model = model;

        }
        /// <summary>
        /// Handles the logs sent by server.
        /// </summary>
        /// <param name="sender">The object which raise the event</param>
        /// <param name="args">ContentEventArgs that contains Logs collection object</param>
        public void recieveLogs(object sender, ContentEventArgs args)
        {
            ICollection<Log> logs = args.GetContent<ICollection<Log>>();
            foreach (Log log in logs)
            {
                Logs.Add(log);
            }

        }
        /// <summary>
        /// Handles with lonely log sent by server.
        /// </summary>
        /// <param name="sender">The object which raise the event</param>
        /// <param name="args">ContentEventArgs that contains Log object</param>
        public void recieveOneLog(object sender, ContentEventArgs args)
        {

            Logs.Add(args.GetContent<Log>());
        }

    }
}
