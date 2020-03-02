using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._M
{
    public class InitialKnowledge : ReactiveObject
    {
        public static int _id = 0;
        private string name;
        private ObservableCollection<KnowledgePair> knowledgePairs = new ObservableCollection<KnowledgePair>();
        private int id;

        public InitialKnowledge()
        {
            _id++;
            this.id = _id;
            this.name = "未命名" + this.id;
        }

        public string Name { get => name; set => this.RaiseAndSetIfChanged(ref name, value); }
        public ObservableCollection<KnowledgePair> KnowledgePairs { get => knowledgePairs; set => knowledgePairs = value; }
        public int Id { get => id; set => id = value; }
    }
}
