using ReactiveUI;
using sbid._M;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class TopoGraph_P_VM : SidePanel_VM
    {
        public static int _id = 0;
        private Connector_VM activeConnector;
        private TopoLinkType topoLinkType = TopoLinkType.OneWay;
        private bool connectorVisible = true;


        // 默认构造时使用默认名称
        public TopoGraph_P_VM()
        {
            _id++;
            this.Name = "拓扑图" + _id;
        }

        // 活动锚点,当按下一个空闲锚点时,该锚点成为面板上唯一的活动锚点,当按下另一空闲锚点进行转移关系连线
        public Connector_VM ActiveConnector { get => activeConnector; set => activeConnector = value; }
        // 当前选中的TopoLinkType枚举
        public TopoLinkType TopoLinkType { get => topoLinkType; set => this.RaiseAndSetIfChanged(ref topoLinkType, value); }
        // 锚点是否可见
        public bool ConnectorVisible { get => connectorVisible; set => this.RaiseAndSetIfChanged(ref connectorVisible, value); }

        #region 拓扑图上的VM操作接口

        // 创建拓扑图TopoLink连线关系（此函数在"_V"下的"TopoConnector_V"中调用）
        public void CreateTopoLinkVM(Connector_VM connectorVM1, Connector_VM connectorVM2)
        {
            // 根据TopoLinkType枚举的类型不同来创建不同的TopoLink
            TopoLink_VM topoLink_VM;
            switch (topoLinkType)
            {
                case TopoLinkType.OneWay:
                    topoLink_VM = new OneWayTopoLink_VM();
                    break;
                case TopoLinkType.TwoWay:
                    topoLink_VM = new TwoWayTopoLink_VM();
                    break;
                default:
                    ResourceManager.mainWindowVM.Tips = "[ERROR]发生在TopoGraph_P_VM.cs";
                    return;
            }

            topoLink_VM.Source = connectorVM1;
            topoLink_VM.Dest = connectorVM2;

            // 锚点反引连接关系
            connectorVM1.ConnectionVM = topoLink_VM;
            connectorVM2.ConnectionVM = topoLink_VM;

            UserControlVMs.Add(topoLink_VM);
        }

        // 删除图上连线关系
        public void BreakTopoLinkVM(Connector_VM connectorVM)
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
