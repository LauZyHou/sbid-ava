using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    // 各个子面板(类图/状态机/攻击树/拓扑图/顺序图)
    // fixme 使用抽象类???
    public class Panel_VM : ViewModelBase
    {
        private string name;
        private ObservableCollection<SidePanel_VM> sidePanelVMs = new ObservableCollection<SidePanel_VM>();
        private SidePanel_VM selectedItem;

        public Panel_VM(string name)
        {
            this.name = name;
        }

        public string Name { get => name; set => name = value; }
        // 集成其下的侧边面板的列表
        public ObservableCollection<SidePanel_VM> SidePanelVMs { get => sidePanelVMs; set => sidePanelVMs = value; }
        // 记录面板中选定的SidePanel项
        public SidePanel_VM SelectedItem
        {
            get => selectedItem;
            set => this.RaiseAndSetIfChanged(ref selectedItem, value);
        }
    }
}
