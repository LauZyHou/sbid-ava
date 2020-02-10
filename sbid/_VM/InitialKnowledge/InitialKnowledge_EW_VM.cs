using sbid._M;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class InitialKnowledge_EW_VM
    {
        private InitialKnowledge initialKnowledge;
        private ObservableCollection<Process> processes = new ObservableCollection<Process>();

        // 要编辑的InitialKnowledge
        public InitialKnowledge InitialKnowledge { get => initialKnowledge; set => initialKnowledge = value; }
        // 集成协议下的所有Process,以用于KnowledgePair的构建和修改
        public ObservableCollection<Process> Processes { get => processes; set => processes = value; }
    }
}
