using System.ComponentModel;
using SharedResources;
using System.Windows.Data;
using System;
using System.Linq;

namespace GUI.Model
{
    public class SettingsModel : ISettingsModel
    {
        private Settings settings;
       
        public SettingsModel(Settings set)
        {
            
            this.settings = set;
            BindingOperations.EnableCollectionSynchronization(settings.Handlers, settings.Handlers);
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
              
                settings = value;
            }
        }

        public Action<object, string> SendRequest
        {
            get;

            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RemoveDirectoryHandler(DirectoryDetails directoryToRemove)
        {
            var directoriesToRemove = settings.Handlers.Where(x => x.DirectoryName == directoryToRemove.DirectoryName);
            DirectoryDetails[] directoriesToRemoveArray = directoriesToRemove.ToArray();
            foreach (DirectoryDetails d in directoriesToRemoveArray)
                settings.Handlers.Remove(d);
        }
    }
}

