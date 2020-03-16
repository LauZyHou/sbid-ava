using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // <Process, Method>
    public class ProcessMethod : ReactiveObject
    {
        private Process process;
        private Method method;

        public ProcessMethod(Process process, Method method)
        {
            this.process = process;
            this.method = method;
        }

        public Process Process { get => process; set => this.RaiseAndSetIfChanged(ref process, value); }
        public Method Method { get => method; set => this.RaiseAndSetIfChanged(ref method, value); }

        public override string ToString()
        {
            return process.Name + "." + method.Name + "[" + method.CryptoSuffix + "]";
        }
    }
}
