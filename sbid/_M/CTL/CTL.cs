using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // 完整的CTL：状态和公式
    public class CTL : ReactiveObject
    {
        private Process process;
        private Formula formula;

        public CTL(Process process, Formula formula)
        {
            this.process = process;
            this.formula = formula;
        }

        public Process Process { get => process; set => this.RaiseAndSetIfChanged(ref process, value); }
        public Formula Formula { get => formula; set => this.RaiseAndSetIfChanged(ref formula, value); }

        public override string ToString()
        {
            return process.RefName.Content + ":" + formula.Content;
        }
    }
}
