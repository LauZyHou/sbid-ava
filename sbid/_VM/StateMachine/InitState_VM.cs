using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    // 初始状态VM
    public class InitState_VM : NetworkItem_VM
    {
        // 构造时添加1个锚点
        public InitState_VM()
        {
            ConnectorVMs = new ObservableCollection<Connector_VM>();
            ConnectorVMs.Add(new Connector_VM() { X = 20, Y = 20 });
        }
    }
}
