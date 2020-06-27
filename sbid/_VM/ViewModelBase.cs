using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;

namespace sbid._VM
{
    public class ViewModelBase : ReactiveObject
    {
        // 判断这个ViewModelBase对象是不是一个NetWorkItem_VM
        // 用于在状态机中区分"线"(直线/箭头)和"元素"(状态/转移块/控制点)
        // 以将"线"设置不可点击也不会显示选中效果，防止影响到"元素"的显示和移动
        public bool IsNetWorkItemVM { get => this is NetworkItem_VM; }
    }
}
