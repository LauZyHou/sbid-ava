using Avalonia;
using ReactiveUI;
using sbid._M;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class StateMachine_P_VM : Network_P_VM
    {
        private State state;
        private Connector_VM activeConnector;
        private ObservableCollection<int> controlPointNums = new ObservableCollection<int>();
        private int controlPointNum = 0;

        // 无参构造只是给xaml中的Design用
        public StateMachine_P_VM()
        {
            init_control_point_nums();
        }

        // 状态机构造时，传入其所精化的状态
        public StateMachine_P_VM(State state)
        {
            this.state = state;
            init_control_point_nums();
        }

        // 精化的状态
        public State State { get => state; }
        // 活动锚点,当按下一个空闲锚点时,该锚点成为面板上唯一的活动锚点,当按下另一空闲锚点进行转移关系连线
        public Connector_VM ActiveConnector { get => activeConnector; set => activeConnector = value; }
        // 控制点数目可选列表
        public ObservableCollection<int> ControlPointNums { get => controlPointNums; }
        // 控制点数目
        public int ControlPointNum { get => controlPointNum; set => this.RaiseAndSetIfChanged(ref controlPointNum, value); }


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

        #region 状态机上的VM操作接口（旧）
        /*
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
        */
        #endregion

        #region 状态机上的VM操作接口（新）

        // 创建转移关系
        public void CreateTransitionVM(Connector_VM connectorVM1, Connector_VM connectorVM2)
        {
            // 控制点X/Y方向间距
            double deltaX = (connectorVM2.Pos.X - connectorVM1.Pos.X) / (controlPointNum + 1);
            double deltaY = (connectorVM2.Pos.Y - connectorVM1.Pos.Y) / (controlPointNum + 1);
            // 当前控制点所在为止，从c1的位置开始
            double nowX = connectorVM1.Pos.X;
            double nowY = connectorVM1.Pos.Y;
            // 从c1连控制点数目个线段
            Connector_VM nowConnector = connectorVM1;
            for (int i = 0; i < controlPointNum; i++)
            {
                nowX += deltaX;
                nowY += deltaY;
                ControlPoint_VM controlPoint_VM = new ControlPoint_VM(nowX, nowY);
                UserControlVMs.Add(controlPoint_VM);
                linkByConnection(nowConnector, controlPoint_VM.ConnectorVMs[0]);
                nowConnector = controlPoint_VM.ConnectorVMs[1];
            }
            // 然后再连接一个箭头到c2即可
            linkByArrow(nowConnector, connectorVM2);
        }

        // 删除锚点上的转移关系
        public void BreakTransitionVM(Connector_VM connectorVM)
        {
            // 先删除这个锚点的直接连线
            Connection_VM connectionVM = connectorVM.ConnectionVM;
            UserControlVMs.Remove(connectionVM);
            // 清除反引
            connectorVM.ConnectionVM = null;
            // 寻找 当前正在处理的“线另一端的锚点”，记录在nowConnector里
            Connector_VM source = connectionVM.Source;
            Connector_VM dest = connectionVM.Dest;
            Connector_VM nowConnector;
            if (source == connectorVM)
            {
                nowConnector = dest;
            }
            else // dest == connectorVM
            {
                nowConnector = source;
            }
            // 循环删除多段线，直到“线另一端的锚点”不再属于“控制点”为止
            while (nowConnector.NetworkItemVM is ControlPoint_VM)
            {
                ControlPoint_VM controlPoint_VM = (ControlPoint_VM)nowConnector.NetworkItemVM;
                UserControlVMs.Remove(controlPoint_VM);
                // 删掉这个锚点，剩下的就是另一个
                controlPoint_VM.ConnectorVMs.Remove(nowConnector);
                // 接下来拿“控制点的”另一个锚点，把控制点控制的另一侧线条删除
                nowConnector = controlPoint_VM.ConnectorVMs[0];
                connectionVM = nowConnector.ConnectionVM;
                UserControlVMs.Remove(connectionVM);
                // 还是检查Source和Dest，以确定线条另一端的锚点
                source = connectionVM.Source;
                dest = connectionVM.Dest;
                if (source == nowConnector)
                {
                    nowConnector = dest;
                }
                else
                {
                    nowConnector = source;
                }
            }
            // 最后，“线另一端的锚点”清除反引
            nowConnector.ConnectionVM = null;
        }
        #endregion

        #region 私有工具

        // 连线动作的封装
        private void linkByConnection(Connector_VM c1, Connector_VM c2)
        {
            Connection_VM connection_VM = new Connection_VM()
            {
                Source = c1,
                Dest = c2
            };
            c1.ConnectionVM = c2.ConnectionVM = connection_VM;
            UserControlVMs.Add(connection_VM);
        }

        // 连箭头动作的封装
        private void linkByArrow(Connector_VM c1, Connector_VM c2)
        {
            Arrow_VM arrow_VM = new Arrow_VM()
            {
                Source = c1,
                Dest = c2
            };
            c1.ConnectionVM = c2.ConnectionVM = arrow_VM;
            UserControlVMs.Add(arrow_VM);
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

        #region 辅助构造

        // 初始化连线控制点数目列表
        private void init_control_point_nums()
        {
            for (int i = 0; i < 5; i++)
                controlPointNums.Add(i);
        }

        #endregion
    }
}
