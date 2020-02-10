using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._M
{
    public class SecurityProperty : ReactiveObject
    {
        private string name;
        private ObservableCollection<Confidential> confidentials = new ObservableCollection<Confidential>();
        private ObservableCollection<Authenticity> authenticities = new ObservableCollection<Authenticity>();

        public SecurityProperty(string name)
        {
            this.name = name;
        }

        public string Name { get => name; set => this.RaiseAndSetIfChanged(ref name, value); }
        public ObservableCollection<Confidential> Confidentials { get => confidentials; set => confidentials = value; }
        public ObservableCollection<Authenticity> Authenticities { get => authenticities; set => authenticities = value; }
    }
}
