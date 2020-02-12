using sbid._M;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class ObjLifeLine_EW_VM
    {
        private SeqObject seqObject;

        // 要编辑的顺序图对象
        public SeqObject SeqObject { get => seqObject; set => seqObject = value; }
    }
}
