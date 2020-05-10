using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    // 正方形拓扑结点
    public class TopoNode_Square_VM : TopoNode_VM
    {

        public TopoNode_Square_VM()
            : base()
        {
            init();
        }

        public TopoNode_Square_VM(double x, double y)
            : base(x, y)
        {
            init();
        }

        #region 私有

        // 辅助构造，处理锚点位置数据
        private void init()
        {
            // 图形左上角点位置
            double baseX = X + 6;
            double baseY = Y + 6;

            // 横纵方向锚点间距
            double delta = 27;

            ConnectorVMs = new ObservableCollection<Connector_VM>();

            // 8个锚点,从左上角锚点中心位置进行位置推算
            ConnectorVMs.Add(new Connector_VM(baseX + 0 * delta, baseY + 0 * delta));
            ConnectorVMs.Add(new Connector_VM(baseX + 1 * delta, baseY + 0 * delta));
            ConnectorVMs.Add(new Connector_VM(baseX + 2 * delta, baseY + 0 * delta));

            ConnectorVMs.Add(new Connector_VM(baseX + 0 * delta, baseY + 1 * delta));
            ConnectorVMs.Add(new Connector_VM(baseX + 2 * delta, baseY + 1 * delta));

            ConnectorVMs.Add(new Connector_VM(baseX + 0 * delta, baseY + 2 * delta));
            ConnectorVMs.Add(new Connector_VM(baseX + 1 * delta, baseY + 2 * delta));
            ConnectorVMs.Add(new Connector_VM(baseX + 2 * delta, baseY + 2 * delta));

            // 将这些锚点所在的NetworkItem_VM回引写入
            foreach (Connector_VM connector_VM in ConnectorVMs)
            {
                connector_VM.NetworkItemVM = this;
            }
        }

        #endregion
    }
}
