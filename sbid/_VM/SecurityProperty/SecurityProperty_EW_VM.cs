using sbid._M;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class SecurityProperty_EW_VM
    {
        private SecurityProperty securityProperty;
        private ObservableCollection<Process> processes = new ObservableCollection<Process>();

        // 要编辑的SecurityProperty
        public SecurityProperty SecurityProperty { get => securityProperty; set => securityProperty = value; }
        // 集成协议中的所有Process
        public ObservableCollection<Process> Processes { get => processes; set => processes = value; }
    }
}
