using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class FinalState_VM : NetworkItem_VM
    {
        public FinalState_VM(double x, double y)
        {
            X = x;
            Y = y;
            ConnectorVMs = new ObservableCollection<Connector_VM>();
            ConnectorVMs.Add(new Connector_VM(X + 20, Y + 4));
        }
    }
}
