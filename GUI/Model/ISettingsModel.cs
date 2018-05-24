using System.ComponentModel;
using SharedResources;
using System.Collections.ObjectModel;
using System;

namespace GUI.Model
{
   public interface ISettingsModel: INotifyPropertyChanged
    {
        Settings Settings { get; set; }
        // ObservableCollection<string> Handlers { get;}
        Action<object,string> SendRequest { get; set; }
    }
}
