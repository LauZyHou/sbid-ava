﻿using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class SequenceDiagram_P_VM : SidePanel_VM
    {
        private static int _id = 1;

        // 默认构造时使用默认名称
        public SequenceDiagram_P_VM()
        {
            this.Name = "顺序图" + _id;
            _id++;
        }
    }
}