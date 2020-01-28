using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class CTLTree_P_VM : SidePanel_VM
    {
        private static int _id = 1;

        // 默认构造时使用默认名称
        public CTLTree_P_VM()
        {
            this.Name = "CTL树" + _id;
            _id++;
        }
    }
}
