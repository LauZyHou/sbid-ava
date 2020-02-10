using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    public class Authenticity : ReactiveObject
    {
        private Process processA;
        private State stateA;
        private Attribute attributeA;
        private Process processB;
        private State stateB;
        private Attribute attributeB;

        public Authenticity(Process processA, State stateA, Attribute attributeA, Process processB, State stateB, Attribute attributeB)
        {
            this.processA = processA;
            this.stateA = stateA;
            this.attributeA = attributeA;
            this.processB = processB;
            this.stateB = stateB;
            this.attributeB = attributeB;
        }

        public Process ProcessA { get => processA; set => this.RaiseAndSetIfChanged(ref processA, value); }
        public State StateA { get => stateA; set => this.RaiseAndSetIfChanged(ref stateA, value); }
        public Attribute AttributeA { get => attributeA; set => this.RaiseAndSetIfChanged(ref attributeA, value); }
        public Process ProcessB { get => processB; set => this.RaiseAndSetIfChanged(ref processB, value); }
        public State StateB { get => stateB; set => this.RaiseAndSetIfChanged(ref stateB, value); }
        public Attribute AttributeB { get => attributeB; set => this.RaiseAndSetIfChanged(ref attributeB, value); }

        public override string ToString()
        {
            return processA.Name + "." + stateA.Name + "." + attributeA.Identifier + " | "
                + processB.Name + "." + stateB.Name + "." + AttributeB.Identifier;
        }
    }
}
