using System.ComponentModel;
using SharedResources;

namespace GUI.Model
{
   public class SettingsModel : ISettingsModel
    {
        private Settings settings;
        public SettingsModel(Settings set)
        {
            
            settings = set;
            settings.PropertyChanged += PropertyChanged;

        }

        public SettingsModel() : this(new Settings()) { }



        public Settings Settings
        {
            get
            {
                return settings;
            }

             set
            {
                //settings.LogName = value.LogName;
                //settings.OutputDirectory = value.OutputDirectory;
                //settings.SourceName = value.SourceName;
                //settings.ThumbnailSize = value.ThumbnailSize;
                //settings.Handlers.Clear();
                //foreach (DirectoryDetails directory in value.Handlers)
                //{
                //    settings.Handlers.Add(directory);
                //}
                settings = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
