using sbid._M;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class Process_VM : NetworkItem_VM
    {
        private static int _id = 1;
        private Process process;

        public Process_VM()
        {
            process = new Process("未命名" + _id);
            _id++;
        }

        public Process Process { get => process; set => process = value; }
    }
}
