using GUI.Model;
using SharedResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    class SettingsViewModel
    {
        public SettingsViewModel(ISettingsModel model)
        {
            this.model = model;
        }


        private ISettingsModel model;
        public Settings Settings
        {
            get
            {
                return model.Settings;
            }
        }

       
    }
}
