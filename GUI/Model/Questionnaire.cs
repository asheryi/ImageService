using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Model
{
    public class Questionnaire : INotifyPropertyChanged
    {
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
        private bool isHandlerSelected=false;
        public bool IsHandlerSelected
        {
            get { return isHandlerSelected; }
            set
            {
                isHandlerSelected = value;
                OnPropertyChanged("IsHandlerSelected");
            }
        }
       
    }
}
