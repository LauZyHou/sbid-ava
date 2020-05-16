using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // 认证性
    public class Authenticity : ReactiveObject
    {
        private Process processA;
        private State stateA;
        private Attribute attributeA;
        private Attribute attributeA_Attr;
        private Process processB;
        private State stateB;
        private Attribute attributeB;
        private Attribute attributeB_Attr;

        public Authenticity(
            Process processA, State stateA, Attribute attributeA, Attribute attributeA_Attr,
            Process processB, State stateB, Attribute attributeB, Attribute attributeB_Attr
            )
        {
            this.processA = processA;
            this.stateA = stateA;
            this.attributeA = attributeA;
            this.attributeA_Attr = attributeA_Attr;
            this.processB = processB;
            this.stateB = stateB;
            this.attributeB = attributeB;
            this.attributeB_Attr = attributeB_Attr;
        }

        public Process ProcessA { get => processA; set => this.RaiseAndSetIfChanged(ref processA, value); }
        public State StateA { get => stateA; set => this.RaiseAndSetIfChanged(ref stateA, value); }
        public Attribute AttributeA { get => attributeA; set => this.RaiseAndSetIfChanged(ref attributeA, value); }
        public Attribute AttributeA_Attr { get => attributeA_Attr; set => this.RaiseAndSetIfChanged(ref attributeA_Attr, value); }
        public Process ProcessB { get => processB; set => this.RaiseAndSetIfChanged(ref processB, value); }
        public State StateB { get => stateB; set => this.RaiseAndSetIfChanged(ref stateB, value); }
        public Attribute AttributeB { get => attributeB; set => this.RaiseAndSetIfChanged(ref attributeB, value); }
        public Attribute AttributeB_Attr { get => attributeB_Attr; set => this.RaiseAndSetIfChanged(ref attributeB_Attr, value); }

        public override string ToString()
        {
            return processA.RefName + "." 
                + stateA.Name + "." 
                + attributeA.Identifier + "." 
                + attributeA_Attr.Identifier + " | "
                + processB.RefName + "." 
                + stateB.Name + "." 
                + AttributeB.Identifier + "."
                + AttributeB_Attr.Identifier;
        }
    }
}
