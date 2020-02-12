using sbid._M;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class AsyncMessage_VM : Message_VM
    {
        public AsyncMessage_VM()
        {
            Message = new Formula("异步消息");
        }
    }
}
