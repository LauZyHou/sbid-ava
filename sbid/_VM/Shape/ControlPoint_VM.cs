using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    // 多段折线的控制点
    public class ControlPoint_VM : NetworkItem_VM
    {
        public ControlPoint_VM(double x, double y)
        {
            X = x;
            Y = y;
            ConnectorVMs = new ObservableCollection<Connector_VM>();
            // 每个控制点只控制两条线，所以就两个锚点，位置重合起来并且不在View上显示就行了
            Connector_VM connector_VM1 = new Connector_VM(x + 5, y + 5)
            {
                NetworkItemVM = this
            };
            ConnectorVMs.Add(connector_VM1);
            Connector_VM connector_VM2 = new Connector_VM(x + 5, y + 5)
            {
                NetworkItemVM = this
            };
            ConnectorVMs.Add(connector_VM2);
        }
    }
}
