using ReactiveUI;
using sbid._M;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class StateMachine_P_VM : Network_P_VM
    {
        private State state;
        private Connector_VM activeConnector;
        private bool connectorVisible = true;

        // 无参构造只是给xaml中的Design用
        public StateMachine_P_VM()
        {
        }

        // 状态机构造时，传入其所精化的状态
        public StateMachine_P_VM(State state)
        {
            this.state = state;
        }

        // 精化的状态
        public State State { get => state; }
        // 活动锚点,当按下一个空闲锚点时,该锚点成为面板上唯一的活动锚点,当按下另一空闲锚点进行转移关系连线
        public Connector_VM ActiveConnector { get => activeConnector; set => activeConnector = value; }
        // 锚点是否可见
        public bool ConnectorVisible { get => connectorVisible; set => this.RaiseAndSetIfChanged(ref connectorVisible, value); }

        #region 对外的初始化调用(在用户创建时需要调用，在从项目文件读取时不可调用)

        public void init_data()
        {
            InitState_VM initStateVM = new InitState_VM(70, 20); // 初始状态
            State_VM stateVM = new State_VM(35, 240); // 白给状态

            // 全加到表里
            UserControlVMs.Add(initStateVM);
            UserControlVMs.Add(stateVM);

            // 创建转移关系,也加到表里
            CreateTransitionVM(initStateVM.ConnectorVMs[0], stateVM.ConnectorVMs[2]);
        }

        #endregion

        #region 状态机上的VM操作接口

        // 创建转移关系
        public void CreateTransitionVM(Connector_VM connectorVM1, Connector_VM connectorVM2)
        {
            Transition_VM transitionVM = new Transition_VM()
            {
                Source = connectorVM1,
                Dest = connectorVM2
            };

            // 锚点反引连接关系
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

        #region 按钮命令

        // 创建普通状态结点
        public void CreateStateVM()
        {
            State_VM stateVM = new State_VM(0, 0);
            UserControlVMs.Add(stateVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的状态结点：" + stateVM.State.Name;
        }

        // 创建终止状态结点
        public void CreateFinalStateVM()
        {
            FinalState_VM finalStateVM = new FinalState_VM(0, 0);
            UserControlVMs.Add(finalStateVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的终止状态结点";
        }

        #endregion
    }
}
