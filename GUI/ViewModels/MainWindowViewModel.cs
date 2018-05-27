using GUI.Model;
using GUI.Views.UserControls;
using Microsoft.Practices.Prism.Commands;
using SharedResources;
using SharedResources.Commands;
using SharedResources.Communication;
using SharedResources.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace GUI.ViewModels
{
    public class MainWindowViewModel
    {
        private SettingsViewModel svm;
        private Client client;
        private IMessageHandler handler;
        LogsView logsView;
        private SettingsView settingsView;
        /// <summary>
        /// MainWindowViewModel constructor.
        /// </summary>
        public MainWindowViewModel()
        {
            
            handler = new CommunicationMessageHandler();
            createSettingsMVVM();
            createLogMVVM();
            client = new Client(handler);
            
  
        }
        /// <summary>
        /// Start the communication with the server.
        /// </summary>
        public void Connect()
        {
            client.Start();
            client.Recieve();
        }
        /// <summary>
        /// Send message to the server.
        /// </summary>
        /// <param name="o">The object which raise the event</param>
        /// <param name="request">The message to be send.</param>
        public void Send(object o,string request)
        {
            Debug.WriteLine(request);
            client.Send(request);
        }
        /// <summary>
        /// Creates the MVVM of the SettingsTab
        /// </summary>
        public void createSettingsMVVM()
        {
            SettingsModel settingsModel = new SettingsModel();
            SettingsViewModel settingsViewModel = new SettingsViewModel(settingsModel);
            settingsModel.SendRequest += Send;
             settingsView = new SettingsView();
           
            handler.RegisterFuncToEvent(CommandEnum.GetConfigCommand, settingsViewModel.recieveSettings);
            handler.RegisterFuncToEvent(CommandEnum.CloseHandlerCommand, settingsViewModel.removeHandler);
            settingsView.DataContext = settingsViewModel;

        }
        /// <summary>
        /// SerttingsView property.
        /// </summary>
        public SettingsView SettingsView
        {
            get
            {
                return settingsView;
            }
        }
        /// <summary>
        /// Creates the MVVM of the LogsTab.
        /// </summary>
        public void createLogMVVM()
        {
            LogsModel logsModel = new LogsModel();
            LogsViewModel logViewModel = new LogsViewModel(logsModel);
            logsView = new LogsView();
            logsView.DataContext = logViewModel;
            handler.RegisterFuncToEvent(CommandEnum.GetAllLogsCommand, logViewModel.recieveLogs);
            handler.RegisterFuncToEvent(CommandEnum.SendLog, logViewModel.recieveOneLog);
        }
        /// <summary>
        /// LogView property.
        /// </summary>
        public LogsView LogView
        {
            get
            {
                return logsView;
            }
        }
        /// <summary>
        /// SettingsViewModel property.
        /// </summary>
        public SettingsViewModel SettingsViewModel
        {
            set
            {
                this.svm = value;
            }
            get
            {
                return this.svm;
            }
        }
    }
}
