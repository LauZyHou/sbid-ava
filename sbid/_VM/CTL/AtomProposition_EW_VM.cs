using sbid._M;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class AtomProposition_EW_VM : ViewModelBase
    {
        private ObservableCollection<Process> processes = new ObservableCollection<Process>();
        private ObservableCollection<Nav> properties = new ObservableCollection<Nav>();
        private AtomProposition atomProposition;

        // 要编辑的原子命题对象
        public AtomProposition AtomProposition { get => atomProposition; set => atomProposition = value; }
        // 所有Process，用于属性导航器选择
        public ObservableCollection<Process> Processes { get => processes; set => processes = value; }
        // 属性选择器例化的Nav
        public ObservableCollection<Nav> Properties { get => properties; }
    }
}
