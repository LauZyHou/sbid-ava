using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    public class TopoNode : ReactiveObject
    {
        public static int _id = 0;
        private string name;
        private Process process;
        private int id;

        public TopoNode()
        {
            _id++;
            this.id = _id;
            this.name = "T" + this.id;
        }

        public string Name { get => name; set => this.RaiseAndSetIfChanged(ref name, value); }
        public Process Process { get => process; set => this.RaiseAndSetIfChanged(ref process, value); }
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
