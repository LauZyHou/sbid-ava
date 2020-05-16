using sbid._M;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class CTLTree_P_VM : SidePanel_VM
    {
        public static int _id = 0;

        // 默认构造时使用默认名称
        public CTLTree_P_VM()
        {
            _id++;
            this.refName = new Formula("CTL树" + _id);
        }
    }
}
