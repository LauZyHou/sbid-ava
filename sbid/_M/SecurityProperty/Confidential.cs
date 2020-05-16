using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    public class Confidential : ReactiveObject
    {
        private Process process;
        private Attribute attribute;

        public Confidential(Process process, Attribute attribute)
        {
            this.process = process;
            this.attribute = attribute;
        }

        public Process Process { get => process; set => this.RaiseAndSetIfChanged(ref process, value); }
        public Attribute Attribute { get => attribute; set => this.RaiseAndSetIfChanged(ref attribute, value); }

        public override string ToString()
        {
            return process.RefName + "." + attribute.Identifier;
        }
    }
}
