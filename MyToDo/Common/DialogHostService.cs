﻿using DryIoc;
using MaterialDesignThemes.Wpf;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
namespace MyToDo.Common
{
    /// <summary>
    /// 对话主机服务（自定义）
    /// </summary>
    public class DialogHostService : DialogService, IDialogHostService
    {
        private readonly IContainerExtension container;

        public DialogHostService(IContainerExtension container) : base(container)
        {
            this.container = container;
        }

     

        public async Task<IDialogResult> ShowDialog(string name, IDialogParameters parameters, string dialogHostName = "Root")
        {
            //没看懂

            parameters ??= new DialogParameters();

            //从容器当中取出弹出窗口的实例
            var content = container.Resolve<object>(name);

            //验证实例的有效性
            if (!(content is FrameworkElement dialogContent))
                throw new NullReferenceException("A dialog's content must be a FrameworkElement");

            if (dialogContent is FrameworkElement view && view.DataContext is null && ViewModelLocator.GetAutoWireViewModel(view) is null)
                ViewModelLocator.SetAutoWireViewModel(view, true);

            if (!(dialogContent.DataContext is IDialogHostAware viewModel))
                throw new NullReferenceException("A dialog's ViewModel must implement the IDialogAware interface");

            viewModel.DialogHostName = dialogHostName;

            DialogOpenedEventHandler eventHandler = (sender, args) =>
            {

                if (viewModel is IDialogHostAware aware)
                {
                    aware.OnDialogOpend(parameters);
                }

                args.Session.UpdateContent(content);
            };

            return (IDialogResult)await DialogHost.Show(dialogContent, viewModel.DialogHostName, eventHandler);
        }

      
    }
}
