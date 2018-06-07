using GUI.Model;
using SharedResources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;
using SharedResources.Communication;
using SharedResources.Commands;
using System.Diagnostics;

namespace GUI.ViewModels
{
    public class SettingsViewModel
    {
        
        private bool isHandlerSelected=false;//Indicates if the button should be enable or disable.
        private DelegateCommand<object> com;//The remove button subscribe to this delegateCommand.
        private IMessageGenerator messageGenerator;//Generates the message to be send to the server.
        private ISettingsModel model;//ISettingsModel.
        /// <summary>
        /// SettingsViewModel constructor.
        /// </summary>
        /// <param name="model">SettingsModel</param>
        public SettingsViewModel(ISettingsModel model)
        {
            this.model = model;
            this.RemoveCommand = new DelegateCommand<object>(this.OnRemove, this.CanRemove);
            com=this.RemoveCommand as DelegateCommand<object>;
            messageGenerator = new CommunicationMessageGenerator();
        }

        /// <summary>
        /// Settings property.
        /// </summary>
        public Settings Settings
        {
            get
            {
                return model.Settings;
            }
        }
        
        DirectoryDetails selectefDirectoryDetails;
        /// <summary>
        /// DirectoryDetails property, the itemSelected event of the listBox
        /// subscribe to this property.
        /// </summary>
        public DirectoryDetails HandlerSelected
        {
            set
            {
               
                
                selectefDirectoryDetails = value;
                bool itemSelected = (selectefDirectoryDetails == null ? false : true);
               
                isHandlerSelected = itemSelected;
                com.RaiseCanExecuteChanged();

            }

        }
        /// <summary>
        ///The remove button subscribe to this property.
        /// </summary>
        public ICommand RemoveCommand { get; private set; }
        /// <summary>
        /// Send remove handler request to the server.
        /// </summary>
        /// <param name="obj"></param>
        private void OnRemove(object obj)
        {
            if (selectefDirectoryDetails != null)
            {
                string send = messageGenerator.Generate(CommandEnum.CloseHandlerCommand, selectefDirectoryDetails);
                model.SendRequest?.Invoke(this, send);
            }
        }
       /// <summary>
       /// Checks if the remove button can be click.
       /// </summary>
       /// <param name="obj"></param>
       /// <returns></returns>
        private bool CanRemove(object obj)
        {

           
            if (!isHandlerSelected)
                return false;
            return true;
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
            model.RemoveDirectoryHandler(directoryToRemove);
        }

    }
}
