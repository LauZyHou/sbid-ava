using sbid._M;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class CTLTree_P_VM : SidePanel_VM
    {
        public static int _id = 0;
        private Connector_VM activeConnector;

        // 默认构造时使用默认名称
        public CTLTree_P_VM()
        {
            _id++;
            this.refName = new Formula("CTL树" + _id);
        }

        // 活动锚点,当按下一个空闲锚点时,该锚点成为面板上唯一的活动锚点,当按下另一空闲锚点进行转移关系连线
        public Connector_VM ActiveConnector { get => activeConnector; set => activeConnector = value; }

        #region CTL树上的VM操作接口

        // 创建树上连线关系
        public void CreateArrowVM(Connector_VM connectorVM1, Connector_VM connectorVM2)
        {
            Arrow_VM arrow_VM = new Arrow_VM();

            arrow_VM.Source = connectorVM1;
            arrow_VM.Dest = connectorVM2;

            // 锚点反引连接关系
            connectorVM1.ConnectionVM = arrow_VM;
            connectorVM2.ConnectionVM = arrow_VM;

            UserControlVMs.Add(arrow_VM);
        }

        // 删除树上连线关系
        public void BreakArrowVM(Connector_VM connectorVM)
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
