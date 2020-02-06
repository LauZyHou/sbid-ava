using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    // 连线VM
    public class Connection_VM : ViewModelBase
    {
        private Connector_VM source;
        private Connector_VM dest;

        public Connector_VM Source { get => source; set => source = value; }
        public Connector_VM Dest { get => dest; set => dest = value; }
    }
}
