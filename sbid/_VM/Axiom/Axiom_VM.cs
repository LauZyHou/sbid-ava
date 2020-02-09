using sbid._M;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class Axiom_VM : NetworkItem_VM
    {
        private static int _id = 1;
        private Axiom axiom;

        public Axiom_VM()
        {
            axiom = new Axiom("未命名" + _id);
            _id++;
        }

        public Axiom Axiom { get => axiom; set => axiom = value; }
    }
}
