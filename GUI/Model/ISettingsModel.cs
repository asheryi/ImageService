using System.ComponentModel;
using SharedResources;
using System.Collections.ObjectModel;
using System;

namespace GUI.Model
{
   public interface ISettingsModel: INotifyPropertyChanged
    {
        /// <summary>
        ///Settings property.
        /// </summary>
        Settings Settings { get; set; }
        /// <summary>
        /// The client communication object subscribe to this action, there for
        /// when the SettingsModel wants to send message to server it can raise
        /// this action.
        /// </summary>
        Action<object,string> SendRequest { get; set; }
        /// <summary>
        /// Removes the directory from the listbox.
        /// </summary>
        /// <param name="directoryToRemove">Directory to remove which sent by the server.</param>
        void RemoveDirectoryHandler(DirectoryDetails directoryToRemove);
    }
}
