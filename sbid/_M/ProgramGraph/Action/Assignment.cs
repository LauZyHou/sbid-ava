using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // 赋值操作
    public class Assignment : Action
    {
        private Param lh;
        private RightHands rh;

        public Param LH { get => lh; set => lh = value; }
        public RightHands RH { get => rh; set => rh = value; }
    }
}
