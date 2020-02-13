using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class Relation_VM : NetworkItem_VM
    {

        public Relation_VM()
        {
            init();
        }

        private void init()
        {
            // 图形左上角点位置(圆的外接正方形左上角点)
            // 左上角锚点中心位置
            double baseX = X + 6;
            double baseY = Y + 6;

            // 圆弧上锚点之间的短delta,短delta+长delta=半径长r
            double d = 2.8 * 3;
            double r = 27;

            ConnectorVMs = new ObservableCollection<Connector_VM>();

            // 顺时针一圈锚点
            ConnectorVMs.Add(new Connector_VM(baseX + 0,
                                              baseY + r));
            ConnectorVMs.Add(new Connector_VM(baseX + d,
                                              baseY + d));
            ConnectorVMs.Add(new Connector_VM(baseX + r,
                                              baseY + 0));
            ConnectorVMs.Add(new Connector_VM(baseX + 2 * r - d,
                                              baseY + d));
            ConnectorVMs.Add(new Connector_VM(baseX + 2 * r,
                                              baseY + r));
            ConnectorVMs.Add(new Connector_VM(baseX + 2 * r - d,
                                              baseY + 2 * r - d));
            ConnectorVMs.Add(new Connector_VM(baseX + r,
                                              baseY + 2 * r));
            ConnectorVMs.Add(new Connector_VM(baseX + d,
                                              baseY + 2 * r - d));
        }
    }
}
