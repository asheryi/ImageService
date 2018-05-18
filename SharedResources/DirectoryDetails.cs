using System.ComponentModel;

namespace SharedResources
{
    public class DirectoryDetails : INotifyPropertyChanged
    {
       public DirectoryDetails(string directoryName)
        {
            this.directoryName = directoryName;
        }
        private string directoryName;
        public string DirectoryName {
            get
            { return directoryName; }
            set
            {
                if(directoryName != value)
                {
                    directoryName = value;

                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
