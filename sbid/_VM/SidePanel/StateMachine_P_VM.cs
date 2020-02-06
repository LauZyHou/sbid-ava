using sbid._M;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class StateMachine_P_VM : SidePanel_VM
    {
        private static int _id = 1;
        private Process process;

        // 默认构造时使用默认名称
        //public StateMachine_P_VM()
        //{
        //    this.Name = "状态机" + _id;
        //    _id++;
        //}

        public StateMachine_P_VM(Process process)
        {
            this.process = process;
            // todo数据绑定
            this.Name = process.Name;
        }

        // 集成所在Process,以反向查询(以用其Name)
        public Process Process { get => process; set => process = value; }
    }
}
