using ReactiveUI;
using sbid._M;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    // 各个子面板下的侧栏面板的超类,各个具体的侧栏面板要继承此类
    public abstract class SidePanel_VM : Network_P_VM
    {
        protected Formula refName;

        public SidePanel_VM()
        {
        }

        // 引用型Name，在SidePanel_VM的子类构造时传入
        // 因为需要考虑StateMachine和Process同步，所以不能直接用string
        public Formula RefName { get => refName; }
    }
}
