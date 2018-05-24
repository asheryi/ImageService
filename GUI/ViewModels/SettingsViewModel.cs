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
        private IMessageGenerator messageGenerator;
        private bool isHandlerSelected=false;
        private DelegateCommand<object> com;
        public SettingsViewModel(ISettingsModel model)
        {
            this.model = model;
            this.SubmitCommand = new DelegateCommand<object>(this.OnSubmit, this.CanSubmit);

            //this.QuestionnaireViewModel = new QuestionnaireViewModel();
            this.ResetCommand = new DelegateCommand(this.OnReset);

           // this.QuestionnaireViewModel.PropertyChanged += PropertyChanged;
            com=this.SubmitCommand as DelegateCommand<object>;

            messageGenerator = new CommunicationMessageGenerator();


        }


        private ISettingsModel model;
        public Settings Settings
        {
            get
            {
                return model.Settings;
            }
        }
        
        DirectoryDetails directoryDetails;
    
        public DirectoryDetails HandlerSelected
        {
            set
            {
               
                
                directoryDetails = value;
                bool itemSelected = (directoryDetails == null ? false : true);
               // QuestionnaireViewModel.Questionnaire.IsHandlerSelected = itemSelected;
                isHandlerSelected = itemSelected;
                com.RaiseCanExecuteChanged();

            }

        }

        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        
            var command = this.SubmitCommand as DelegateCommand<object>;
            command.RaiseCanExecuteChanged();
        }
        public ICommand SubmitCommand { get; private set; }

        public ICommand ResetCommand { get; private set; }

        public QuestionnaireViewModel QuestionnaireViewModel { get; set; }

        private void OnSubmit(object obj)
        {
            if (directoryDetails != null)
            {
                string send = messageGenerator.Generate(CommandEnum.CloseHandlerCommand, directoryDetails);
                string sss = ObjectConverter.Serialize(new CommunicationMessage(CommandEnum.CloseHandlerCommand, directoryDetails.DirectoryName));
                Debug.WriteLine(sss + "   WOOOOOOOOORKKKKKS");
                Debug.WriteLine(send + "   NOT ");

                model.SendRequest?.Invoke(this, send);
            }
        }
       
        private bool CanSubmit(object obj)
        {

            //if (!this.QuestionnaireViewModel.Questionnaire.IsHandlerSelected)
            //{
            //    return false;
            //}
            if (!isHandlerSelected)
                return false;
            return true;
        }


        private void OnReset()
        {
            this.QuestionnaireViewModel.Questionnaire = new Questionnaire();
        }

      
        public void recieveSettings(object sender, ContentEventArgs args)
        {

            Settings settings = args.GetContent<Settings>();
            Settings.updateSettings(settings);
            
        }
        public void removeHandler(object sender, ContentEventArgs args)
        {
            Debug.WriteLine("WOW IT's HEREEEEEEE");
            DirectoryDetails directoryToRemove = args.GetContent<DirectoryDetails>();

            model.RemoveDirectoryHandler(directoryToRemove);

            //Stack<DirectoryDetails> directoryToRemoveStack = new Stack<DirectoryDetails>();
            //foreach (DirectoryDetails d in Settings.Handlers)
            //{
            //    if (d.DirectoryName == directoryToRemove.DirectoryName)
            //        directoryToRemoveStack.Push(d);

            //}
            //while (directoryToRemoveStack.Count > 0)
            //    Settings.Handlers.Remove(directoryToRemoveStack.Pop());


        }

    }
}
