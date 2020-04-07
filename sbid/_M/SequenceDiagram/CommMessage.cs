using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // 序列图上的通信消息
    public class CommMessage : ReactiveObject
    {
        private CommMethod commMethod;

        public CommMessage(CommMethod commMethod)
        {
            this.commMethod = commMethod;
        }

        // 里面组合一个来自Process的通信方法
        public CommMethod CommMethod { get => commMethod; set => this.RaiseAndSetIfChanged(ref commMethod, value); }
    }
}
