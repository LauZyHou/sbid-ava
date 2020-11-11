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
        public static int _id = 0;
        private Protocol protocol;
        private ObservableCollection<Panel_VM> panelVMs = new ObservableCollection<Panel_VM>();
        private Panel_VM selectedItem;

        public Protocol_VM()
        {
            _id++;
            protocol = new Protocol("协议" + _id);

            Panel_VM classDiagramPVM = new Panel_VM("概览");
            //classDiagramPVM.SidePanelVMs.Add(new UserType_P_VM());
            classDiagramPVM.SidePanelVMs.Add(new ClassDiagram_P_VM());
            classDiagramPVM.SelectedItem = classDiagramPVM.SidePanelVMs[0];
            panelVMs.Add(classDiagramPVM);

            panelVMs.Add(new Panel_VM("状态机"));

            Panel_VM topoGraphPVM = new Panel_VM("拓扑图");
            //topoGraphPVM.SidePanelVMs.Add(new TopoGraph_P_VM());// 添加一个默认面板
            //topoGraphPVM.SelectedItem = topoGraphPVM.SidePanelVMs[0];// 设置默认选中项
            panelVMs.Add(topoGraphPVM);

            Panel_VM attackTreePVM = new Panel_VM("攻击树");
            //attackTreePVM.SidePanelVMs.Add(new AttackTree_P_VM());
            //attackTreePVM.SidePanelVMs.Add(new AttackTree_P_VM());
            //attackTreePVM.SelectedItem = attackTreePVM.SidePanelVMs[0];
            panelVMs.Add(attackTreePVM);

            Panel_VM ctlTreePVM = new Panel_VM("CTL语法树");
            //ctlTreePVM.SidePanelVMs.Add(new CTLTree_P_VM());
            //ctlTreePVM.SelectedItem = ctlTreePVM.SidePanelVMs[0];
            panelVMs.Add(ctlTreePVM);

            Panel_VM sequenceDiagramPVM = new Panel_VM("序列图");
            //sequenceDiagramPVM.SidePanelVMs.Add(new SequenceDiagram_P_VM());
            //sequenceDiagramPVM.SelectedItem = sequenceDiagramPVM.SidePanelVMs[0];
            panelVMs.Add(sequenceDiagramPVM);

            Panel_VM accessControlPVM = new Panel_VM("访问控制图");
            panelVMs.Add(accessControlPVM);

            // 设置本协议的默认选中项为第一项"概览"
            selectedItem = panelVMs[0];
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

        //public static bool IsNull(Protocol_VM p)
        //{
        //    return p == null;
        //}
    }
}
