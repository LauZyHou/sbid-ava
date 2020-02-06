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
        public State_VM()
        {
            ConnectorVMs = new ObservableCollection<Connector_VM>();
            for (int i = 0; i < 6; i++)
            {
                ConnectorVMs.Add(new Connector_VM() { X = 100, Y = 100 });
            }
        }
    }
}
