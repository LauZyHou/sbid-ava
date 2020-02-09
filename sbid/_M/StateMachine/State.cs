using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    public class State : ReactiveObject
    {
        private string name;

        public State(string name)
        {
            this.name = name;
        }

        public string Name { get => name; set => this.RaiseAndSetIfChanged(ref name, value); }
    }
}
