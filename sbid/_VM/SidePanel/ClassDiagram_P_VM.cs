using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    // 类图面板
    public class ClassDiagram_P_VM : SidePanel_VM
    {
        private static int _id = 1;

        // 默认构造时使用默认名称
        public ClassDiagram_P_VM()
        {
            this.Name = "类图" + _id;
            _id++;
        }
    }
}
