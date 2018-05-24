using SharedResources;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace WebApplication2.Models
{
    // Stores the settings as they appear in the appconfig.
    public class Settings : INotifyPropertyChanged
    {
        public Settings()
        {

            Handlers = new ObservableCollection<DirectoryDetails>();
        }

        public Settings(string outputDirectory, string sourceName, string logName, int thumbnailSize)
        {
            initialize(outputDirectory, sourceName, logName, thumbnailSize);
            Handlers = new ObservableCollection<DirectoryDetails>();
        }
        public void initialize(string outputDirectory, string sourceName, string logName, int thumbnailSize)
        {
            this.OutputDirectory = outputDirectory;
            this.SourceName = sourceName;
            this.LogName = logName;
            this.ThumbnailSize = thumbnailSize;
        }
        public void updateSettings(Settings settings)
        {
            initialize(settings.outputDirectory, settings.sourceName, settings.logName, settings.thumbnailSize);
            handlers.Clear();
            foreach (DirectoryDetails directory in settings.Handlers)
            {
                this.Handlers.Add(directory);
            }
        }
        private string outputDirectory;
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Output Directory:")]
        public string OutputDirectory {
            get { return outputDirectory; }
            set
            {
                outputDirectory = value;
                OnPropertyChanged("OutputDirectory");
            }
        }

        private string sourceName;
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Source Name:")]
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
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Log Name:")]
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
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Thumbnail Size:")]
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

            set
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
