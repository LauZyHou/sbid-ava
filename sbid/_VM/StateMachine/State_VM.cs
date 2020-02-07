using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    // 普通状态VM
    public class State_VM : NetworkItem_VM
    {
        // 构造时添加6个锚点
        public State_VM(double x, double y)
        {
            X = x;
            Y = y;
            ConnectorVMs = new ObservableCollection<Connector_VM>();

            // 左上角锚点中心位置
            double baseX = X + 6;
            double baseY = Y + 8;
            // 横纵方向锚点间距
            double deltaX = 24.5;
            double deltaY = 16;

            // 14个锚点,从左上角锚点中心位置进行位置推算
            ConnectorVMs.Add(new Connector_VM(baseX + 0 * deltaX, baseY + 0 * deltaY));
            ConnectorVMs.Add(new Connector_VM(baseX + 1 * deltaX, baseY + 0 * deltaY));
            ConnectorVMs.Add(new Connector_VM(baseX + 2 * deltaX, baseY + 0 * deltaY));
            ConnectorVMs.Add(new Connector_VM(baseX + 3 * deltaX, baseY + 0 * deltaY));
            ConnectorVMs.Add(new Connector_VM(baseX + 4 * deltaX, baseY + 0 * deltaY));

            ConnectorVMs.Add(new Connector_VM(baseX + 0 * deltaX, baseY + 1 * deltaY));
            ConnectorVMs.Add(new Connector_VM(baseX + 4 * deltaX, baseY + 1 * deltaY));

            ConnectorVMs.Add(new Connector_VM(baseX + 0 * deltaX, baseY + 2 * deltaY));
            ConnectorVMs.Add(new Connector_VM(baseX + 0 * deltaX, baseY + 2 * deltaY));

            ConnectorVMs.Add(new Connector_VM(baseX + 0 * deltaX, baseY + 3 * deltaY));
            ConnectorVMs.Add(new Connector_VM(baseX + 1 * deltaX, baseY + 3 * deltaY));
            ConnectorVMs.Add(new Connector_VM(baseX + 2 * deltaX, baseY + 3 * deltaY));
            ConnectorVMs.Add(new Connector_VM(baseX + 3 * deltaX, baseY + 3 * deltaY));
            ConnectorVMs.Add(new Connector_VM(baseX + 4 * deltaX, baseY + 3 * deltaY));
        }
    }
}
