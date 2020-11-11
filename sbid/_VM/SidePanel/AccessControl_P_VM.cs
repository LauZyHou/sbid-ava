using sbid._M;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class AccessControl_P_VM : SidePanel_VM
    {
        public static int _id = 0;
        private Connector_VM activeConnector;

        public AccessControl_P_VM()
        {
            _id++;
            this.refName = new Formula("访问控制" + _id);
        }

        // 活动锚点,当按下一个空闲锚点时,该锚点成为面板上唯一的活动锚点,当按下另一空闲锚点进行转移关系连线
        public Connector_VM ActiveConnector { get => activeConnector; set => activeConnector = value; }

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

        #region 状态机上的VM操作接口（新）

        // 创建转移关系
        public void CreateTransitionVM(Connector_VM connectorVM1, Connector_VM connectorVM2)
        {
            linkByArrow(connectorVM1, connectorVM2);
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
            // 最后，“线另一端的锚点”清除反引
            nowConnector.ConnectionVM = null;
        }

        #endregion

        #region 私有工具

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
    }
}
