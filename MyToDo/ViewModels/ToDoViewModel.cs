using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Extension;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    public class ToDoViewModel : NavigationViewModel
    {


        private readonly IDialogHostService dialogHost;

        public DelegateCommand<string> ExecuteCommand { get; private set; }
        public DelegateCommand<ToDoDto> SelectedCommand { get; private set; }
        public DelegateCommand<ToDoDto> DeleteCommand { get; private set; }

        private ObservableCollection<ToDoDto> todoDto;
        private readonly IToDoService service;

        public ToDoViewModel(IToDoService service, IContainerProvider containerProvider) : base(containerProvider)
        {
            TodoDtos = new ObservableCollection<ToDoDto>();


            dialogHost = containerProvider.Resolve<IDialogHostService>();
            ExecuteCommand = new DelegateCommand<string>(Execute);
            SelectedCommand = new DelegateCommand<ToDoDto>(Selected);
            DeleteCommand = new DelegateCommand<ToDoDto>(Delete);


            this.service = service;

        }

        private async void Delete(ToDoDto dto)
        {

            try
            {

                var dialogResult = await dialogHost.Question("温馨提示", $"您确认要删除待办事项:{dto.Title}吗？");

                if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK)
                    return;

                UpdateLoading(true);

                var deleteResult = await service.DeleteAsync(dto.Id);

                if (deleteResult.Status)
                {
                    var model = TodoDtos.FirstOrDefault(t => t.Id.Equals(dto.Id));

                    if (model != null)
                    {
                        TodoDtos.Remove(model);
                    }

                }
            }
            catch (Exception ex) { }
            finally
            {
                UpdateLoading(false);
            }


        }

        private void Execute(string obj)
        {
            switch (obj)
            {

                case "新增": Add(); break;
                case "查询": GetDataAsync(); break;
                case "保存": Save(); break;
            }
        }



        private bool isRightDrawerOpen;

        private string search;

        /// <summary>
        /// 搜索条件
        /// </summary>
        public string Search
        {
            get { return search; }
            set { search = value; RaisePropertyChanged(); }
        }



        private int selectedIndex;


        /// <summary>
        /// 下拉列表选中状态值
        /// </summary>
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; RaisePropertyChanged(); }
        }




        private ToDoDto currentDto;

        /// <summary>
        /// 编辑选中对象/新增对象时
        /// </summary>
        public ToDoDto CurrentDto
        {
            get { return currentDto; }
            set { currentDto = value; RaisePropertyChanged(); }
        }


        /// <summary>
        /// 右侧编辑窗口是否展开
        /// </summary>
        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }


        /// <summary>
        /// 添加待办
        /// </summary>
        private void Add()
        {
            CurrentDto = new ToDoDto();
            IsRightDrawerOpen = true;
        }



        /// <summary>
        /// 选中窗口打开加载数据
        /// </summary>
        /// <param name="dto"></param>
        private async void Selected(ToDoDto dto)
        {


            try
            {
                UpdateLoading(true);
                var todoResult = await service.GetFirstOfDefaultAsync(dto.Id);

                if (todoResult.Status)
                {

                    CurrentDto = todoResult.Result;
                    IsRightDrawerOpen = true;
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                UpdateLoading(false);
            }


        }
        /// <summary>
        /// 查询方法
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void Query()
        {

        }

        /// <summary>
        ///保存
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private async void Save()
        {
            if (string.IsNullOrWhiteSpace(currentDto.Title) || string.IsNullOrWhiteSpace(currentDto.Content))
                return;

            UpdateLoading(true);
            try
            {
                if (currentDto.Id > 0)
                {
                    var updateResult = await service.UpdateAsync(currentDto);

                    if (updateResult.Status)
                    {
                        var todo = TodoDtos.FirstOrDefault(t => t.Id == CurrentDto.Id);
                        if (todo != null)
                        {
                            todo.Title = currentDto.Title;
                            todo.Content = currentDto.Content;
                            todo.Status = currentDto.Status;


                        }

                    }
                    IsRightDrawerOpen = false;
                }
                else
                {
                    var addResult = await service.AddAsync(currentDto);

                    if (addResult.Status)
                    {
                        TodoDtos.Add(addResult.Result);
                        IsRightDrawerOpen = false;

                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                UpdateLoading(false);
            }

        }


        public ObservableCollection<ToDoDto> TodoDtos
        {
            get { return todoDto; }
            set { todoDto = value; RaisePropertyChanged(); }
        }



        /// <summary>
        /// 获取数据
        /// </summary>
        async void GetDataAsync()
        {
            UpdateLoading(true);//加载界面


            int? status = SelectedIndex == 0 ? null : SelectedIndex == 2 ? 1 : 0;

            var todoResult = await service.GetAllFilterAsync(new ToDoParameter()
            {
                PageIndex = 0,
                PageSize = 100,
                Search = Search,
                Status = status,


            });

            if (todoResult.Status)

                TodoDtos.Clear();

            foreach (var item in todoResult.Result.Items)
            {
                TodoDtos.Add(item);


            }

            UpdateLoading(false);//加载完毕




        }


        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            GetDataAsync();
        }
    }
}