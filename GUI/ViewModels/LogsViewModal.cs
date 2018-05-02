using GUI.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GUI.ViewModels
{
    class LogsViewModal
    {
        private LogsModel model;
        public ObservableCollection<Log> Logs
        {
            get
            {
                return model.Logs;
            }
        }

        public LogsViewModal(LogsModel model)
        {
            this.model = model;
        }


    }
}
