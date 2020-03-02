using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._M
{
    public class SafetyProperty : ReactiveObject
    {
        public static int _id = 0;
        private string name;
        private ObservableCollection<Formula> cTLs = new ObservableCollection<Formula>();
        private ObservableCollection<Formula> invariants = new ObservableCollection<Formula>();
        private int id;

        public SafetyProperty()
        {
            _id++;
            this.id = _id;
            this.name = "未命名" + this.id;
        }

        public string Name { get => name; set => this.RaiseAndSetIfChanged(ref name, value); }
        public ObservableCollection<Formula> CTLs { get => cTLs; set => cTLs = value; }
        public ObservableCollection<Formula> Invariants { get => invariants; set => invariants = value; }
        public int Id { get => id; set => id = value; }
    }
}
