using System.ComponentModel;
using System.Windows.Data;
using System;
using System.Linq;
using SharedResources;

namespace WebApplication2.Models.Models
{
    public class SettingsModel
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

        /// <summary>
        /// Removes the directory from the listbox.
        /// </summary>
        /// <param name="directoryToRemove">Directory to remove which sent by the server.</param>
        private void RemoveDirectoryHandler(DirectoryDetails directoryToRemove)
        {
            var directoriesToRemove = settings.Handlers.Where(x => x.DirectoryName == directoryToRemove.DirectoryName);
            DirectoryDetails[] directoriesToRemoveArray = directoriesToRemove.ToArray();
            foreach (DirectoryDetails d in directoriesToRemoveArray)
                settings.Handlers.Remove(d);
        }


        /// <summary>
        /// Recieve Settings from the server.
        /// </summary>
        /// <param name="sender">The object which raise the event</param>
        /// <param name="args">ContentEventArgs that contains Settings object</param>
        public void recieveSettings(object sender, ContentEventArgs args)
        {

            Settings settings = args.GetContent<Settings>();
            Settings.updateSettings(settings);

        }
        /// <summary>
        /// Removes directoryDetails from listbox.
        /// </summary>
        /// <param name="sender">The object which raise the event</param>
        /// <param name="args">ContentEventArgs that contains DirectoryDetails object</param>
        public void removeHandler(object sender, ContentEventArgs args)
        {
            DirectoryDetails directoryToRemove = args.GetContent<DirectoryDetails>();
            RemoveDirectoryHandler(directoryToRemove);
        }



    }
}

