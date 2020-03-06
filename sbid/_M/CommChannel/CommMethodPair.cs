using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // 表达选择的CommMethod，<进程模板,进程模板下的CommMethod>的序对
    public class CommMethodPair : ReactiveObject
    {
        private Process process;
        private CommMethod commMethod;

        public CommMethodPair(Process process, CommMethod commMethod)
        {
            this.process = process;
            this.commMethod = commMethod;
        }

        public Process Process { get => process; set => this.RaiseAndSetIfChanged(ref process, value); }
        public CommMethod CommMethod { get => commMethod; set => this.RaiseAndSetIfChanged(ref commMethod, value); }

        public override string ToString()
        {
            return process.Name + "." + commMethod.Name;
        }
    }
}
