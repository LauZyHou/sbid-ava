using sbid._M;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class Message_EW_VM
    {
        private Formula message;

        // 要编辑的消息
        public Formula Message { get => message; set => message = value; }
    }
}
