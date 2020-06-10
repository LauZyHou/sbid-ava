using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class LogicRelation_EW_VM : ViewModelBase
    {
        private LogicRelation_VM logicRelation_VM;

        // 注意这里传了整个VM，而不是里面的LogicRelation枚举，为了能让枚举外面套个对象
        public LogicRelation_VM LogicRelation_VM { get => logicRelation_VM; set => logicRelation_VM = value; }
    }
}
