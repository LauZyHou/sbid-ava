using sbid._M;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class TopoNode_EW_VM
    {
        private TopoNode topoNode;
        private ObservableCollection<Process> processes = new ObservableCollection<Process>();

        // 要编辑的TopoNode
        public TopoNode TopoNode { get => topoNode; set => topoNode = value; }
        // 集成协议下的所有Process
        public ObservableCollection<Process> Processes { get => processes; set => processes = value; }
    }
}
