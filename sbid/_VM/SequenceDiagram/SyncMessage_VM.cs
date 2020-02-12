using Avalonia;
using sbid._M;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class SyncMessage_VM : Message_VM
    {
        public SyncMessage_VM()
        {
            Message = new Formula("同步消息");
        }
    }
}
