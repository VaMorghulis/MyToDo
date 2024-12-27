using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.Events
{

    /// <summary>
    /// 定义事件
    /// </summary>
    public class UpdateLoadingEvent:PubSubEvent<UpdateModel>
    {

    }


    public class UpdateModel
    {
        public bool IsOpen { get; set; }
    }

}
