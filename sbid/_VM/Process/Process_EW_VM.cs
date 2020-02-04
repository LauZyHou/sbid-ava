using sbid._M;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class Process_EW_VM
    {
        private Process process;
        private ObservableCollection<_M.Type> types = new ObservableCollection<_M.Type>();

        // 要编辑的Process
        public Process Process { get => process; set => process = value; }
        // 所有Type对象(包括int, bool),作为Attribute/Method/CommMethod的可用类型
        public ObservableCollection<sbid._M.Type> Types { get => types; set => types = value; }
    }
}
