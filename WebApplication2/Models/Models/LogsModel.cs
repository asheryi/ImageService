using System.ComponentModel;
using System.Collections.ObjectModel;
using WebApplication2.Models.Logging;
using System.Windows.Data;
using WebApplication2.Model.Communication;
using System.Collections.Generic;
using SharedResources;
using System.Linq;

namespace WebApplication2.Models.Models
{
    class LogsModel
    {
      
        private ICollection<Log> logs;//Contains the log sent by the server.
       /// <summary>
       /// Logs collection property.
       /// </summary>
        public ICollection<Log> Logs
        {
            get
            {
                return filterLogsBy(filter);
                //return logs;
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
            this.Logs = new List<Log>();
            Filter = "";
            //Allows more than one thread access the collection.
           

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
        public ICollection<Log> filterLogsBy(string filter)
        {
            ICollection<Log> FilterLogs = new List<Log>();
            if(filter == "")
            {
                return logs;
            }
            Log[] found = logs.Where(log => log.Type.ToString() == filter).ToArray();
            return found;
        }
       private string filter;
        public string Filter
        {
            set
            {
                filter = value;
            }
            get
            {
                return filter;
            }
        }
    }
}
