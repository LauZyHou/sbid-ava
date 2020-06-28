using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // 表达选择的CommMethod，<进程模板,进程模板下的CommMethod>的序对
    public class CommMethodPair : ReactiveObject
    {
        public static int _id = 0;
        private Process processA;
        private Process processB;
        private CommMethod commMethodA;
        private CommMethod commMethodB;
        private bool privacy;
        private int id;

        public CommMethodPair(Process processA, CommMethod commMethodA, Process processB, CommMethod commMethodB, bool privacy)
        {
            _id++;
            this.id = _id;
            this.processA = processA;
            this.processB = processB;
            this.commMethodA = commMethodA;
            this.commMethodB = commMethodB;
            this.privacy = privacy;
        }

        public Process ProcessA { get => processA; set => this.RaiseAndSetIfChanged(ref processA, value); }
        public Process ProcessB { get => processB; set => this.RaiseAndSetIfChanged(ref processB, value); }
        public CommMethod CommMethodA { get => commMethodA; set => this.RaiseAndSetIfChanged(ref commMethodA, value); }
        public CommMethod CommMethodB { get => commMethodB; set => this.RaiseAndSetIfChanged(ref commMethodB, value); }
        public bool Privacy
        {
            get => privacy;

            set
            {
                this.RaiseAndSetIfChanged(ref privacy, value);
                this.RaisePropertyChanged(nameof(Symbol));
            }
        }
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

        public override string ToString()
        {
            return "(" + processA.RefName + "." + commMethodA.Name + ")->(" + processB.RefName + "." + commMethodB.Name + ")";
        }

        public string Symbol { get => privacy ? "-->" : "|->"; }
    }
}
