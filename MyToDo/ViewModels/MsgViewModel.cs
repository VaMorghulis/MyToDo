using MaterialDesignThemes.Wpf;
using MyToDo.Common;
using System;
using Prism.DryIoc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace MyToDo.ViewModels
{
    public class MsgViewModel : BindableBase, IDialogHostAware
    {


        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; RaisePropertyChanged(); }
        }

        private string content;

        public string Content
        {
            get { return content; }
            set { content = value; RaisePropertyChanged(); }
        }


        public MsgViewModel()
        {
            SaveCommand = new DelegateCommand(Save);

            CancelCommand = new DelegateCommand(Cancel);
        }

        private void Cancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName)) { 
              
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No));
            }
        }

        private void Save()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                //DialogParameters param = new DialogParameters();
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK));
            }


        }

        public string DialogHostName { get; set; } = "Root";
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        DelegateCommand IDialogHostAware.SaveCommand { get; set; }
        DelegateCommand IDialogHostAware.CancelCommand { get; set; }

        public void OnDialogOpend(IDialogParameters dialogParameters)
        {

            if (dialogParameters.ContainsKey("Title"))
                title = dialogParameters.GetValue<string>("Title");

            if (dialogParameters.ContainsKey("Content"))
                content = dialogParameters.GetValue<string>("Content");

        }

      
    }
}
