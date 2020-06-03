using ReactiveUI;

using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // 表达Knowledge,<进程模板,进程模板下的Attribute>
    public class Knowledge : ReactiveObject
    {
        private Process process;
        private Attribute attribute;

        public Knowledge(Process process, Attribute attribute)
        {
            this.process = process;
            this.attribute = attribute;
        }

        public Process Process { get => process; set => this.RaiseAndSetIfChanged(ref process, value); }
        public Attribute Attribute { get => attribute; set => this.RaiseAndSetIfChanged(ref attribute, value); }

        public override string ToString()
        {
            return "<" + process.RefName + ", " + attribute.Identifier + ">";
        }
    }
}
