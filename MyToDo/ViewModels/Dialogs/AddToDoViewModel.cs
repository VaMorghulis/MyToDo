using MaterialDesignThemes.Wpf;
using MyToDo.Common;
using MyToDo.Shared.Dtos;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace MyToDo.ViewModels.Dialogs
{
    public class AddToDoViewModel : BindableBase, IDialogHostAware
    {

        public AddToDoViewModel()
        {
            SaveCommand = new DelegateCommand(Save);

            CancelCommand = new DelegateCommand(Cancel);
        }

        private ToDoDto model;

        /// <summary>
        /// 新增或编辑的事情
        /// </summary>
        public ToDoDto Model
        {
            get { return model; }
            set { model = value; RaisePropertyChanged(); }
        }


        private void Cancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No));
        }

        /// <summary>
        /// 确定
        /// </summary>
        private void Save()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                DialogParameters param = new DialogParameters();
                param.Add("value", model);
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, param));
            }


        }

        public string DialogHostName { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        DelegateCommand IDialogHostAware.SaveCommand { get; set; }
        DelegateCommand IDialogHostAware.CancelCommand { get; set; }
      

        public void OnDialogOpend(IDialogParameters dialogParameters)
        {

        }

      

        
    }

}

