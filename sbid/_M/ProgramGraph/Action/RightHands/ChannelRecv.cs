using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // 通道接收操作
    public class ChannelRecv : RightHands
    {
        private Channel c;
        private Param p;

        public Channel C { get => c; set => c = value; }
        public Param P { get => p; set => p = value; }
    }
}
