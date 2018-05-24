using SharedResources.Logging;
using GUI.Model;
using GUI.ViewModels;
using GUI.Views.UserControls;
using ImageService.Logging.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System;
using System.Diagnostics;
using System.Windows.Data;
using System.Collections.Generic;
using SharedResources;
using System.Windows.Media;
using SharedResources.Commands;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
           
            InitializeComponent();
            MainWindowViewModel mainWindowVM = new MainWindowViewModel();
            SettingsViewTab.Content = mainWindowVM.SettingsView;
            LogsViewTab.Content = mainWindowVM.LogView;
           
            try
            {
                mainWindowVM.Connect();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                this.Background=Brushes.Gray;
            }      
        }
    }
}
