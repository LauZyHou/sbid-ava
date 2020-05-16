using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // 公式类,实际是对字符串的封装(为了保证传递引用)
    // 如状态机的Action和公理的公式都可直接使用此类
    public class Formula : ReactiveObject
    {
        private string content;

        public Formula(string content)
        {
            this.content = content;
        }

        public string Content { get => content; set => this.RaiseAndSetIfChanged(ref content, value); }

        public override string ToString()
        {
            return this.content;
        }
    }
}
