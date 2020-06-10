using ReactiveUI;
using sbid._M;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class CTLRelation_EW_VM : ViewModelBase
    {
        private CTLRelation_VM cTLRelation_VM;

        // 注意这里传了整个VM，而不是里面的CTLRelation枚举，为了能让枚举外面套个对象
        public CTLRelation_VM CTLRelation_VM { get => cTLRelation_VM; set => cTLRelation_VM = value; }
    }
}
