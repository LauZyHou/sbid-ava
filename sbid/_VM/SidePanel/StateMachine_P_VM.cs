using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM.SidePanel
{
    public class StateMachine_P_VM : SidePanel_VM
    {
        private static int _id = 1;

        // 默认构造时使用默认名称
        public StateMachine_P_VM()
        {
            this.Name = "状态机" + _id;
            _id++;
        }
    }
}
