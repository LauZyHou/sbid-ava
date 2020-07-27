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
        private ObservableCollection<Nav> cTLProperties = new ObservableCollection<Nav>();
        private ObservableCollection<Nav> invProperties = new ObservableCollection<Nav>();

        // 要编辑的功能安全性质
        public SafetyProperty SafetyProperty { get => safetyProperty; set => safetyProperty = value; }
        // 所有Process
        public ObservableCollection<Process> Processes { get => processes; set => processes = value; }
        // CTL页的属性选择器例化的Nav
        public ObservableCollection<Nav> CTLProperties { get => cTLProperties; }
        // 不变性页的属性选择器例化的Nav
        public ObservableCollection<Nav> InvProperties { get => invProperties; }
    }
}
