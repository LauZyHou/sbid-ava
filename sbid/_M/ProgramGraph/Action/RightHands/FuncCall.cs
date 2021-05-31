using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // 函数调用
    public class FuncCall : RightHands
    {
        private Function func;
        private List<Param> ps;

        public Function Func { get => func; set => func = value; }
        public List<Param> Ps { get => ps; set => ps = value; }
    }
}
