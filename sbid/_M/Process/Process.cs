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

        // 进程模板名
        public string Name { get => name; set => this.RaiseAndSetIfChanged(ref name, value); }
        // 成员参数列表
        public ObservableCollection<Attribute> Attributes { get => attributes; set => attributes = value; }
        // 方法列表
        public ObservableCollection<Method> Methods { get => methods; set => methods = value; }
        // 通信方法列表
        public ObservableCollection<CommMethod> CommMethods { get => commMethods; set => commMethods = value; }
    }
}
