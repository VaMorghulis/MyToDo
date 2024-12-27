using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using Prism.Commands;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    public class IndexViewModel : NavigationViewModel
    {

        private readonly IToDoService toDoService;
        private readonly IMemoService memoService;
        public IndexViewModel(IContainerProvider provider, IDialogHostService dialog) : base(provider)
        {
            CreateTaskBars();
            Title = $"你好， {DateTime.Now.GetDateTimeFormats('D')[1].ToString()}";
            ToDoDtos = new ObservableCollection<ToDoDto>();
            MemoDtos = new ObservableCollection<MemoDto>();
            ExecuteCommand = new DelegateCommand<string>(Execute);
            this.toDoService = provider.Resolve<IToDoService>();
            this.memoService = provider.Resolve<IMemoService>();
            this.dialog = dialog;
        }


        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; RaisePropertyChanged(); }
        }


        private void Execute(string obj)
        {
            switch (obj)
            {

                case "新增备忘录": AddMemo(); break;
                case "新增待办": AddToDo(); break;
                default:
                    break;
            }
        }



        #region


        private ObservableCollection<TaskBar> taskBars;

        private ObservableCollection<MemoDto> memoDtos;

        private ObservableCollection<ToDoDto> toDoDtos;
        private readonly IDialogHostService dialog;

        public ObservableCollection<TaskBar> TaskBars
        {
            get { return taskBars; }
            set { taskBars = value; RaisePropertyChanged(); }

        }

        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set { memoDtos = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<ToDoDto> ToDoDtos
        {
            get { return toDoDtos; }
            set { toDoDtos = value; RaisePropertyChanged(); }

        }
        public DelegateCommand<string> ExecuteCommand { get; private set; }
        #endregion


        async void AddToDo()
        {
            var dialogResult = await dialog.ShowDialog("AddToDoView", null);

            if (dialogResult.Result == ButtonResult.OK)
            {

                var todo = dialogResult.Parameters.GetValue<ToDoDto>("Value");

                if (todo.Id > 0)
                {

                }
                else
                {
                    var addResult = await toDoService.AddAsync(todo);
                    if (addResult.Status)
                    {
                        ToDoDtos.Add(addResult.Result);
                    }
                }
            }
        }

          async  void AddMemo()
            {
               var dialogResult=await dialog.ShowDialog("AddMemoView", null);


            if (dialogResult.Result == ButtonResult.OK)
            {

                var memo = dialogResult.Parameters.GetValue<MemoDto>("Value");

                if (memo.Id > 0)
                {

                }
                else
                {
                    var addResult = await memoService.AddAsync(memo);
                    if (addResult.Status)
                    {
                       MemoDtos.Add(addResult.Result);
                    }
                }
            }
        }





            void CreateTaskBars()
            {
                taskBars = new ObservableCollection<TaskBar>();


                TaskBars.Add(new TaskBar() { Icon = "ClockFast", Title = "汇总", Content = "9", Color = "#FFCA0FF", Target = "" });
                TaskBars.Add(new TaskBar() { Icon = "ClockCheckOutline", Title = "已完成", Content = "9", Color = "#FF1CA0FF", Target = "" });
                TaskBars.Add(new TaskBar() { Icon = "ChartLineVariant", Title = "完成率", Content = "100%", Color = "#FF2CA0FF", Target = "" });
                TaskBars.Add(new TaskBar() { Icon = "PlaylistStar", Title = "备忘录", Content = "19", Color = "#FF3CA0FF", Target = "" });

            }




        }
    }
