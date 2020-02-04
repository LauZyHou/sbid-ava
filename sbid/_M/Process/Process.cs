using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._M
{
    // 进程模板
    public class Process : ReactiveObject
    {
        private string name;
        private ObservableCollection<Attribute> attributes = new ObservableCollection<Attribute>();
        private ObservableCollection<Method> methods = new ObservableCollection<Method>();
        private ObservableCollection<CommMethod> commMethods = new ObservableCollection<CommMethod>();

        public Process(string name)
        {
            this.name = name;
        }

        public string Name { get => name; set => this.RaiseAndSetIfChanged(ref name, value); }
        public ObservableCollection<Attribute> Attributes { get => attributes; set => attributes = value; }
        public ObservableCollection<Method> Methods { get => methods; set => methods = value; }
        public ObservableCollection<CommMethod> CommMethods { get => commMethods; set => commMethods = value; }
    }
}
