using sbid._M;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class StateMachine_P_VM : SidePanel_VM
    {
        private Process process;
        private Connector_VM activeConnector;

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
            State_VM stateVM = new State_VM(40, 140); // 白给状态

            // 全加到表里
            UserControlVMs.Add(initStateVM);
            UserControlVMs.Add(stateVM);

            //Transition_VM connection_VM = new Transition_VM() // 转移关系
            //{
            //    Source = initStateVM.ConnectorVMs[0],
            //    Dest = state_VM.ConnectorVMs[2]
            //};
            //// 锚点的反引
            //initStateVM.ConnectorVMs[0].ConnectionVM = connection_VM;
            //state_VM.ConnectorVMs[2].ConnectionVM = connection_VM;

            //UserControlVMs.Add(connection_VM);

            // 创建转移关系,也加到表里
            CreateTransitionVM(initStateVM.ConnectorVMs[0], stateVM.ConnectorVMs[2]);
        }

        // 集成所在Process,以反向查询(以用其Name)
        public Process Process { get => process; set => process = value; }

        // 活动锚点,当按下一个空闲锚点时,该锚点成为面板上唯一的活动锚点,当按下另一空闲锚点进行转移关系连线
        public Connector_VM ActiveConnector { get => activeConnector; set => activeConnector = value; }

        #region 状态机上的VM操作接口

        // 创建转移关系
        public void CreateTransitionVM(Connector_VM connectorVM1, Connector_VM connectorVM2)
        {
            Transition_VM transitionVM = new Transition_VM()
            {
                Source = connectorVM1,
                Dest = connectorVM2
            };

            connectorVM1.ConnectionVM = transitionVM;
            connectorVM2.ConnectionVM = transitionVM;

            UserControlVMs.Add(transitionVM);
        }

        // 删除锚点上的转移关系
        public void BreakTransitionVM(Connector_VM connectorVM)
        {
            // 要删除的转移关系
            Connection_VM connectionVM = connectorVM.ConnectionVM;

            // 从图形上移除
            UserControlVMs.Remove(connectionVM);

            // 找转移关系的两端锚点(有一个是自己,但是不用管哪个是自己)
            Connector_VM source = connectionVM.Source;
            Connector_VM dest = connectionVM.Dest;

            // 清除反引
            source.ConnectionVM = dest.ConnectionVM = null;
        }

        #endregion
    }
}
