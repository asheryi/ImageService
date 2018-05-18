using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SharedResources
{
    public class Settings : INotifyPropertyChanged
    {
        public Settings()
        {

            Handlers = new ObservableCollection<DirectoryDetails>();
        }

        public Settings(string outputDirectory, string sourceName, string logName, int thumbnailSize)
        {
            this.outputDirectory = outputDirectory;
            this.sourceName = sourceName;
            this.logName = logName;
            this.thumbnailSize = thumbnailSize;
            Handlers = new ObservableCollection<DirectoryDetails>();
        
        }

        private string outputDirectory;
        public string OutputDirectory {
            get { return outputDirectory; }
            set
            {
                outputDirectory = value;
                OnPropertyChanged("OutputDirectory");
            }
        }

        private string sourceName;
        public string SourceName
        {
            get { return sourceName; }
            set
            {
                sourceName = value;
                OnPropertyChanged("SourceName");
            }
        }
        private string logName;
        public string LogName
        {
            get { return logName; }
            set
            {
                logName = value;
                OnPropertyChanged("LogName");
            }
        }
        private int thumbnailSize;
        public int ThumbnailSize
        {
            get { return thumbnailSize; }
            set
            {
                thumbnailSize = value;
                OnPropertyChanged("ThumbnailSize");
            }
        }

        private ObservableCollection<DirectoryDetails> handlers;
      

        public ObservableCollection<DirectoryDetails> Handlers
        {
            get
            {
                return handlers;
            }

            private set
            {
                handlers = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
