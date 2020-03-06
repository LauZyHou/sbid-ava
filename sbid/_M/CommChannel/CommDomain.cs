using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._M
{
    // 通信域，里面由若干CommMethodPair组成，还需要有域的名字
    public class CommDomain : ReactiveObject
    {
        private string name;
        private ObservableCollection<CommMethodPair> commMethodPairs; // 这里不创建,在构造时传入

        public CommDomain(string name, ObservableCollection<CommMethodPair> commMethodPairs)
        {
            this.name = name;
            this.commMethodPairs = commMethodPairs;
        }

        public string Name { get => name; set => this.RaiseAndSetIfChanged(ref name, value); }
        public ObservableCollection<CommMethodPair> CommMethodPairs { get => commMethodPairs; set => this.RaiseAndSetIfChanged(ref commMethodPairs, value); }

        public override string ToString()
        {
            string ret = name + ":";
            foreach (CommMethodPair commMethodPair in commMethodPairs)
            {
                ret += " " + commMethodPair;
            }
            return ret;
        }
    }
}
