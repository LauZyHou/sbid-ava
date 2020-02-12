using sbid._M;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class ReturnMessage_VM : Message_VM
    {
        public ReturnMessage_VM()
        {
            Message = new Formula("返回消息");
        }
    }
}
