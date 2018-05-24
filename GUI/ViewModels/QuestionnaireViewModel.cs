using GUI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    public class QuestionnaireViewModel : INotifyPropertyChanged
    {
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            
        }
        private Questionnaire questionnaire;

        public QuestionnaireViewModel()
        {
            this.Questionnaire = new Questionnaire();
            Questionnaire.PropertyChanged +=
       delegate (Object sender, PropertyChangedEventArgs e)
       {
           NotifyPropertyChanged(e.PropertyName);
       };
        }

        public Questionnaire Questionnaire
        {
            get { return this.questionnaire; }
            set
            {
                this.questionnaire = value;
            }
        }

    }
}
