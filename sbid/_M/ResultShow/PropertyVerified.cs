using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // 验证的性质
    public class PropertyVerified : ReactiveObject
    {
        private string description;
        private bool verified;
        private string counterExample;

        public PropertyVerified(string description, bool verified, string counterExample)
        {
            this.description = description;
            this.verified = verified;
            this.counterExample = counterExample;
        }

        public string Description { get => description; set => description = value; }
        public bool Verified { get => verified; set => verified = value; }
        public string CounterExample { get => counterExample; set => counterExample = value; }
    }
}
