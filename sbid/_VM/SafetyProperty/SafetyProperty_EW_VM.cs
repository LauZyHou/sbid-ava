using sbid._M;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class SafetyProperty_EW_VM : ViewModelBase
    {
        private SafetyProperty safetyProperty;
        private ObservableCollection<Process> processes = new ObservableCollection<Process>();

        // 要编辑的功能安全性质
        public SafetyProperty SafetyProperty { get => safetyProperty; set => safetyProperty = value; }
        // 所有Process
        public ObservableCollection<Process> Processes { get => processes; set => processes = value; }
    }
}
