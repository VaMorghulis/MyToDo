using DryIoc;
using MaterialDesignThemes.Wpf;
using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Extension;
using Prism.Events;
using Prism.Ioc;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace MyToDo.Views
{
    /// <summary>
    /// </summary>
    public partial class MainView : Window
    {
        private readonly IDialogHostService dialogHostService;

        public MainView(IEventAggregator aggregator,IDialogHostService dialogHostService)
        {
            InitializeComponent();
            

            //注册提示消息
            aggregator.RegisterMessage(arg => {

                MainSnackbar.MessageQueue.Enqueue(arg);
            });

            //注册等待消息窗口 订阅事件
            aggregator.Register(arg =>
            {

                DialogHost.IsOpen=arg.IsOpen;

                if (DialogHost.IsOpen) {

                    DialogHost.DialogContent = new ProgressView();
                }
            });



            btnMin.Click += (s, e) => { this.WindowState = WindowState.Minimized; };
            btnMax.Click += (s, e) =>
            {
                if (this.WindowState == WindowState.Maximized)
                    this.WindowState = WindowState.Normal;
                else
                    this.WindowState = WindowState.Maximized;
            };

            btnClose.Click += async (s, e) =>
            {
             var dialogResult=  await  dialogHostService.Question("温馨提示", "确认退出系统？");

                if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK)
                    return;

                this.Close();
            };

            ColorZone.MouseMove += (s, e) =>
            {

                if (e.LeftButton == MouseButtonState.Pressed)
                    this.DragMove();
            };


            ColorZone.MouseDoubleClick += (s, e) =>
            {
                if (this.WindowState == WindowState.Normal)
                    this.WindowState = WindowState.Maximized;
                else
                    this.WindowState = WindowState.Normal;
            };

            menuBar.SelectionChanged += (s, e) =>
            {

                drawerHost.IsLeftDrawerOpen = false;
            };
            this.dialogHostService = dialogHostService;
        }
    }
}
