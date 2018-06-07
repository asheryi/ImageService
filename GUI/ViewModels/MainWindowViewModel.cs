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
   
        public MainWindowViewModel()
        {
            
            handler = new CommunicationMessageHandler();
            createSettingsMVVM();
            createLogMVVM();
            client = new Client(handler);
  
        }
        public void Connect()
        {
            client.Start();
            client.Recieve();
        }
        public void Send(object o,string request)
        {
            Debug.WriteLine(request);
            client.Send(request);
        }
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
        public SettingsView SettingsView
        {
            get
            {
                return settingsView;
            }
        }
        public void createLogMVVM()
        {
            LogsModel logsModel = new LogsModel();
            LogsViewModel logViewModel = new LogsViewModel(logsModel);
            logsView = new LogsView();
            logsView.DataContext = logViewModel;
            handler.RegisterFuncToEvent(CommandEnum.GetAllLogsCommand, logViewModel.recieveLogs);
            handler.RegisterFuncToEvent(CommandEnum.SendLog, logViewModel.recieveOneLog);
        }
        public LogsView LogView
        {
            get
            {
                return logsView;
            }
        }
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
