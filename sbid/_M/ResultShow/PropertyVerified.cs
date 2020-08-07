using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // 验证的性质
    public class PropertyVerified : ReactiveObject
    {
        private string type;
        private string description;
        private bool passed;
        private string counterExample;

        public PropertyVerified(string type, string description, bool passed, string counterExample)
        {
            this.type = type;
            this.description = description;
            this.passed = passed;
            this.counterExample = counterExample;
        }

        // 性质类型，如"机密性"
        public string Type { get => type; set => type = value; }
        // 性质描述
        public string Description { get => description; set => description = value; }
        // 性质验证是否通过
        public bool Passed { get => passed; set => passed = value; }
        // 反例
        public string CounterExample { get => counterExample; set => counterExample = value; }
        // 是否通过的字符串描述
        public string PassedSymbol { get => passed ? "√" : "×"; }
    }
}
