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
        private ObservableCollection<CTL> cTLs = new ObservableCollection<CTL>();
        private ObservableCollection<Formula> invariants = new ObservableCollection<Formula>();
        private int id;

        public SafetyProperty()
        {
            _id++;
            this.id = _id;
            this.name = "Sft" + this.id;
        }

        public string Name { get => name; set => this.RaiseAndSetIfChanged(ref name, value); }
        public ObservableCollection<CTL> CTLs { get => cTLs; set => cTLs = value; }
        public ObservableCollection<Formula> Invariants { get => invariants; set => invariants = value; }
        public int Id
        {
            get => id;
            set
            {
                id = value;
                if (value > _id)
                    _id = value;
            }
        }
    }
}
