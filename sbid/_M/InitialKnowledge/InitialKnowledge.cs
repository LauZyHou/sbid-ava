using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._M
{
    public class InitialKnowledge : ReactiveObject
    {
        private string name;
        private ObservableCollection<KnowledgePair> knowledgePairs = new ObservableCollection<KnowledgePair>();

        public InitialKnowledge(string name)
        {
            this.name = name;
        }

        public string Name { get => name; set => this.RaiseAndSetIfChanged(ref name, value); }
        public ObservableCollection<KnowledgePair> KnowledgePairs { get => knowledgePairs; set => knowledgePairs = value; }
    }
}
