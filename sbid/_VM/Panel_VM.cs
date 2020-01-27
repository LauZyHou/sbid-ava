using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    // 各个面板(类图/状态机/攻击树/拓扑图/顺序图)的超类
    // fixme 使用抽象类
    public class Panel_VM
    {
        private string name;

        public Panel_VM(string name)
        {
            this.name = name;
        }

        public string Name { get => name; set => name = value; }
    }
}
