using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // 表达选择的CommMethod，<进程模板,进程模板下的CommMethod>的序对
    public class CommMethodPair : ReactiveObject
    {
        private Process processA;
        private Process processB;
        private CommMethod commMethodA;
        private CommMethod commMethodB;

        public CommMethodPair(Process processA, CommMethod commMethodA, Process processB, CommMethod commMethodB)
        {
            this.processA = processA;
            this.processB = processB;
            this.commMethodA = commMethodA;
            this.commMethodB = commMethodB;
        }

        public Process ProcessA { get => processA; set => this.RaiseAndSetIfChanged(ref processA, value); }
        public Process ProcessB { get => processB; set => this.RaiseAndSetIfChanged(ref processB, value); }
        public CommMethod CommMethodA { get => commMethodA; set => this.RaiseAndSetIfChanged(ref commMethodA, value); }
        public CommMethod CommMethodB { get => commMethodB; set => this.RaiseAndSetIfChanged(ref commMethodB, value); }

        public override string ToString()
        {
            return "(" + processA.Name + "." + commMethodA.Name + ")->(" + processB.Name + "." + commMethodB.Name + ")";
        }
    }
}
