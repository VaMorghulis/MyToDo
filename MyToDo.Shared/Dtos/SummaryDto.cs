using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Shared.Dtos
{
    public class SummaryDto : BaseDto
    {
        private int sum;
        private int completedCount;
        private int memoCount;
        private string completedRatio;


        private ObservableCollection<ToDoDto> toDoList;

        private ObservableCollection<MemoDto> memoList;


        /// <summary>
        /// 备忘录列表
        /// </summary>
        public ObservableCollection<MemoDto> MemoList
        { get => memoList; set { memoList = value; OnPropertyChange(); } }

        /// <summary>
        /// 待办事项列表
        /// </summary>
        public ObservableCollection<ToDoDto> ToDoList
        { get => toDoList; set { toDoList = value; OnPropertyChange(); } }

        public int Sum { get => sum; set { sum = value; OnPropertyChange(); } }
        public int CompletedCount { get => completedCount; set { completedCount = value; OnPropertyChange(); } }
        public int MemoCount { get => memoCount; set { memoCount = value; OnPropertyChange(); } }
        public string CompletedRatio { get => completedRatio; set { completedRatio = value; OnPropertyChange(); } }
    }
}
