using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // 公钥私钥对
    public class KeyPair : ReactiveObject
    {
        private Process pubProcess;
        private Attribute pubKey;
        private Process secProcess;
        private Attribute secKey;

        public KeyPair(Process pubProcess, Attribute pubKey, Process secProcess, Attribute secKey)
        {
            this.pubProcess = pubProcess;
            this.pubKey = pubKey;
            this.secProcess = secProcess;
            this.secKey = secKey;
        }

        public Process PubProcess { get => pubProcess; set => this.RaiseAndSetIfChanged(ref pubProcess, value); }
        public Attribute PubKey { get => pubKey; set => this.RaiseAndSetIfChanged(ref pubKey, value); }
        public Process SecProcess { get => secProcess; set => this.RaiseAndSetIfChanged(ref secProcess, value); }
        public Attribute SecKey { get => secKey; set => this.RaiseAndSetIfChanged(ref secKey, value); }

        public override string ToString()
        {
            return pubProcess.RefName.Content + "." + pubKey.Identifier + "=>" +
                secProcess.RefName.Content + "." + secKey.Identifier;
        }
    }
}
