using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class ObjLifeLine_VM : NetworkItem_VM
    {
        public ObjLifeLine_VM()
        {
            double baseX = X + 70;
            double baseY = Y + 54;
            double deltaY = 18;

            ConnectorVMs = new ObservableCollection<Connector_VM>();
            for (int i = 0; i < 20; i++)
            {
                ConnectorVMs.Add(new Connector_VM(baseX, baseY + i * deltaY));
            }
        }

        public ObjLifeLine_VM(double x, double y)
        {
            X = x;
            Y = y;

            double baseX = x + 70;
            double baseY = y + 54;
            double deltaY = 18;

            ConnectorVMs = new ObservableCollection<Connector_VM>();
            for (int i = 0; i < 20; i++)
            {
                ConnectorVMs.Add(new Connector_VM(baseX, baseY + i * deltaY));
            }
        }
    }
}
