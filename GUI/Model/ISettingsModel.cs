using System.ComponentModel;
using SharedResources;
using System.Collections.ObjectModel;

namespace GUI.Model
{
    interface ISettingsModel: INotifyPropertyChanged
    {
        Settings Settings { get; set; }
        // ObservableCollection<string> Handlers { get;}
    }
}
