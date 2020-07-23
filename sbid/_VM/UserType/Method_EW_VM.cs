using sbid._M;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    // 方法实现的窗体
    public class Method_EW_VM : ViewModelBase
    {
        private Method method;

        // 要实现的方法
        public Method Method { get => method; set => method = value; }
    }
}
