using ReactiveUI;
using sbid._M;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class Protocol_VM : ViewModelBase
    {
        private static int _id = 1;
        private Protocol protocol;
        private ObservableCollection<Panel_VM> panelVMs = new ObservableCollection<Panel_VM>();
        private Panel_VM selectedItem;

        public Protocol_VM()
        {
            protocol = new Protocol("协议" + _id);
            _id++;
            panelVMs.Add(new Panel_VM("概览"));
            panelVMs.Add(new Panel_VM("状态机"));
            panelVMs.Add(new Panel_VM("拓扑图"));
            panelVMs.Add(new Panel_VM("攻击树"));
            panelVMs.Add(new Panel_VM("CTL公式的抽象语法树"));
        }

        public Protocol Protocol { get => protocol; set => protocol = value; }
        // 集成这个协议下的各个面板的ViewModel
        public ObservableCollection<Panel_VM> PanelVMs { get => panelVMs; set => panelVMs = value; }
        // 记录选中的Tab,用于在切换协议时不影响到各自所选中的面板
        public Panel_VM SelectedItem
        {
            get => selectedItem;
            set => this.RaiseAndSetIfChanged(ref selectedItem, value);
        }
    }
}
