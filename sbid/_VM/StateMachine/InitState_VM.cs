using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    // 初始状态VM
    public class InitState_VM : NetworkItem_VM
    {
        // 无参构造给xaml里Desgin用
        public InitState_VM()
        {
            ConnectorVMs = new ObservableCollection<Connector_VM>();
            ConnectorVMs.Add(new Connector_VM(X + 20, Y + 44));
        }

        // 带位置的构造,构造时添加一个锚点,并为锚点设定好初始位置(todo 自动计算)
        // 数值取决于图形排布,在当前实现是手动写进去的,如果InitState_V的锚点布局改了这里也要改
        public InitState_VM(double x, double y)
        {
            X = x;
            Y = y;
            ConnectorVMs = new ObservableCollection<Connector_VM>();
            ConnectorVMs.Add(new Connector_VM(X + 20, Y + 44));
        }
    }
}
