using sbid._M;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class StateMachine_P_VM : SidePanel_VM
    {
        private Process process;

        // 无参构造只是给xaml中的Design用
        public StateMachine_P_VM()
        {
            init_data();
        }

        public StateMachine_P_VM(Process process)
        {
            this.process = process;
            // todo数据绑定
            this.Name = process.Name;

            init_data();
        }

        private void init_data()
        {
            InitState_VM initStateVM = new InitState_VM(70, 20); // 初始状态
            State_VM state_VM = new State_VM(40, 140); // 白给状态
            Connection_VM connection_VM = new Arrow_VM() // 连线(箭头)
            {
                Source = initStateVM.ConnectorVMs[0],
                Dest = state_VM.ConnectorVMs[2]
            };
            // 全加到表里
            UserControlVMs.Add(initStateVM);
            UserControlVMs.Add(state_VM);
            UserControlVMs.Add(connection_VM);
        }

        // 集成所在Process,以反向查询(以用其Name)
        public Process Process { get => process; set => process = value; }
    }
}
