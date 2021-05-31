using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // 通道发送操作
    public class ChannelSend : Action
    {
        private Channel c;
        private List<Param> ps;

        public Channel C { get => c; set => c = value; }
        public List<Param> Ps { get => ps; set => ps = value; }
    }
}
