using MaterialDesignThemes.Wpf;
using MyToDo.Common;
using MyToDo.Extension;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using RestSharp.Validation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    public class MemoViewModel : NavigationViewModel
    {

        private readonly IDialogHostService dialogHost;
        public DelegateCommand<string> ExecuteCommand { get; private set; }
        public DelegateCommand<MemoDto> SelectedCommand { get; private set; }
        public DelegateCommand<MemoDto> DeleteCommand { get; private set; }

        private ObservableCollection<MemoDto> memoDtos;


        private readonly IMemoService service;

        public MemoViewModel(IMemoService service, IContainerProvider containerProvider) : base(containerProvider)
        {

            dialogHost = containerProvider.Resolve<IDialogHostService>();
            MemoDtos = new ObservableCollection<MemoDto>();

            ExecuteCommand = new DelegateCommand<string>(Execute);
            SelectedCommand = new DelegateCommand<MemoDto>(Selected);
            DeleteCommand = new DelegateCommand<MemoDto>(Delete);
            this.service = service;

        }


        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set { memoDtos = value; RaisePropertyChanged(); }
        }


        private async void Delete(MemoDto dto)
        {
            try
            {

                var dialogResult = await dialogHost.Question("温馨提示", $"您确认要删除备忘录事项:{dto.Title}吗？");

                if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK)
                    return;
                UpdateLoading(true);

                var deleteResult = await service.DeleteAsync(dto.Id);

                if (deleteResult.Status)
                {
                    var model = MemoDtos.FirstOrDefault(t => t.Id.Equals(dto.Id));

                    if (model != null)
                    {
                        MemoDtos.Remove(model);
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



        private MemoDto currentDto;

        /// <summary>
        /// 编辑选中对象/新增对象时
        /// </summary>
        public MemoDto CurrentDto
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
            CurrentDto = new MemoDto();
            IsRightDrawerOpen = true;
        }



        /// <summary>
        /// 选中窗口打开加载数据
        /// </summary>
        /// <param name="dto"></param>
        private async void Selected(MemoDto dto)
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
                        var memo = MemoDtos.FirstOrDefault(t => t.Id == CurrentDto.Id);
                        if (memo != null)
                        {
                            memo.Title = currentDto.Title;
                            memo.Content = currentDto.Content;



                        }

                    }
                    IsRightDrawerOpen = false;
                }
                else
                {
                    var addResult = await service.AddAsync(currentDto);

                    if (addResult.Status)
                    {
                        MemoDtos.Add(addResult.Result);
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




        /// <summary>
        /// 获取数据
        /// </summary>
        async void GetDataAsync()
        {
            UpdateLoading(true);//加载界面


            var todoResult = await service.GetAllAsync(new QueryParameter()
            {
                PageIndex = 0,
                PageSize = 100,
                Search = Search



            });

            if (todoResult.Status)

                MemoDtos.Clear();

            foreach (var item in todoResult.Result.Items)
            {
                MemoDtos.Add(item);


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
