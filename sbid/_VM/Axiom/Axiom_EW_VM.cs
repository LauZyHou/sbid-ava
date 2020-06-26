using ReactiveUI;
using sbid._M;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class Axiom_EW_VM : ViewModelBase
    {
        private Axiom axiom;
        private ObservableCollection<Process> processes = new ObservableCollection<Process>();

        // 要编辑的公理
        public Axiom Axiom { get => axiom; set => axiom = value; }
        // 所有Process
        public ObservableCollection<Process> Processes { get => processes; set => processes = value; }
    }
}
