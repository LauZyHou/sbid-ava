using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // PG的转移边 A -> B
    public class Trans
    {
        private Loc a;
        private Loc b;

        public Loc A { get => a; set => a = value; }
        public Loc B { get => b; set => b = value; }
    }
}
