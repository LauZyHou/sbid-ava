using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._M
{
    public class SecurityProperty : ReactiveObject
    {
        public static int _id = 0;
        private string name;
        private ObservableCollection<Confidential> confidentials = new ObservableCollection<Confidential>();
        private ObservableCollection<Authenticity> authenticities = new ObservableCollection<Authenticity>();
        private ObservableCollection<Integrity> integrities = new ObservableCollection<Integrity>();
        private ObservableCollection<Availability> availabilities = new ObservableCollection<Availability>();
        private int id;

        public SecurityProperty()
        {
            _id++;
            this.id = _id;
            this.name = "Scr" + this.id;
        }

        public string Name { get => name; set => this.RaiseAndSetIfChanged(ref name, value); }
        public ObservableCollection<Confidential> Confidentials { get => confidentials; set => confidentials = value; }
        public ObservableCollection<Authenticity> Authenticities { get => authenticities; set => authenticities = value; }
        public ObservableCollection<Integrity> Integrities { get => integrities; set => integrities = value; }
        public ObservableCollection<Availability> Availabilities { get => availabilities; }
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
