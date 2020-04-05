using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._M
{
    public class TopoNode : ReactiveObject
    {
        public static int _id = 0;
        private string name;
        private Process process;
        private ObservableCollection<Instance> properties = new ObservableCollection<Instance>();
        private int id;

        public TopoNode()
        {
            _id++;
            this.id = _id;
            this.name = "T" + this.id;
        }

        public string Name { get => name; set => this.RaiseAndSetIfChanged(ref name, value); }
        public Process Process { get => process; set => this.RaiseAndSetIfChanged(ref process, value); }
        // 属性列表，用于例化Process的实例，参见ReferenceInstance
        public ObservableCollection<Instance> Properties { get => properties; }
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
