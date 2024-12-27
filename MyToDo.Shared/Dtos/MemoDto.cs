using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Shared.Dtos
{

    /// <summary>
    /// 备忘录
    /// </summary>
    public class MemoDto:BaseDto
    {
        private string title;

        private string content;

       
        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChange(); }
        }

        public string Content
        {
            get { return content; }
            set { content = value; OnPropertyChange(); }
        }
       
    }
}
