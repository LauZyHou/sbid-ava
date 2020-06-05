using sbid._M;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class AtomProposition_EW_VM : ViewModelBase
    {
        private AtomProposition atomProposition;

        // 要编辑的原子命题对象
        public AtomProposition AtomProposition { get => atomProposition; set => atomProposition = value; }
    }
}
