using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // 序列图"对象"
    public class SeqObject : ReactiveObject
    {
        private Process process;

        public SeqObject(Process process)
        {
            this.process = process;
        }

        public Process Process { get => process; set => this.RaiseAndSetIfChanged(ref process, value); }
    }
}
