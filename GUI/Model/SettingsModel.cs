using System.ComponentModel;
using SharedResources;
using System.Windows.Data;
using System;
using System.Linq;

namespace GUI.Model
{
    public class SettingsModel : ISettingsModel
    {
        
        private Settings settings;//Contains settings details sent by server.
       /// <summary>
       /// SettingsModel constructor.
       /// </summary>
       /// <param name="set">Contains settings details sent by server.</param>
        public SettingsModel(Settings set)
        {
            
            this.settings = set;
            //Allows more than one thread access the collection.
            BindingOperations.EnableCollectionSynchronization(settings.Handlers, settings.Handlers);
            settings.PropertyChanged += PropertyChanged;
        }
        /// <summary>
        /// Override the defualt constructor, and reuse code.
        /// </summary>
        public SettingsModel() : this(new Settings()) { }


        /// <summary>
        /// Settings property.
        /// </summary>
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
        /// <summary>
        /// The client communication object subscribe to this action, there for
        /// when the SettingsModel wants to send message to server it can raise
        /// this action.
        /// </summary>
        public Action<object, string> SendRequest
        {
            get;

            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Removes the directory from the listbox.
        /// </summary>
        /// <param name="directoryToRemove">Directory to remove which sent by the server.</param>
        public void RemoveDirectoryHandler(DirectoryDetails directoryToRemove)
        {
            var directoriesToRemove = settings.Handlers.Where(x => x.DirectoryName == directoryToRemove.DirectoryName);
            DirectoryDetails[] directoriesToRemoveArray = directoriesToRemove.ToArray();
            foreach (DirectoryDetails d in directoriesToRemoveArray)
                settings.Handlers.Remove(d);
        }
    }
}

