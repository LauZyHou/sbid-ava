using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._M
{
    public class SafetyProperty : ReactiveObject
    {
        private string name;
        private ObservableCollection<Formula> cTLs = new ObservableCollection<Formula>();
        private ObservableCollection<Formula> invariants = new ObservableCollection<Formula>();

        public SafetyProperty(string name)
        {
            this.name = name;
        }

        public string Name { get => name; set => this.RaiseAndSetIfChanged(ref name, value); }
        public ObservableCollection<Formula> CTLs { get => cTLs; set => cTLs = value; }
        public ObservableCollection<Formula> Invariants { get => invariants; set => invariants = value; }
    }
}
