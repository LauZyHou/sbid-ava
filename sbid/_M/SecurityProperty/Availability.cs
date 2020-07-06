using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // 可用性
    public class Availability : ReactiveObject
    {
        private Process process;
        private State state;

        public Availability(Process process, State state)
        {
            this.process = process;
            this.state = state;
        }

        public Process Process { get => process; set => this.RaiseAndSetIfChanged(ref process, value); }
        public State State { get => state; set => this.RaiseAndSetIfChanged(ref state, value); }

        public override string ToString()
        {
            return process.RefName.Content + "." + state.Name;
        }
    }
}
