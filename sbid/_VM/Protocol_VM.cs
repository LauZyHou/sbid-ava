using sbid._M;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class Protocol_VM
    {
        private static int _id = 1;
        private Protocol protocol;

        public Protocol_VM()
        {
            protocol = new Protocol("协议" + _id);
            _id++;
        }

        public Protocol Protocol { get => protocol; set => protocol = value; }
    }
}
