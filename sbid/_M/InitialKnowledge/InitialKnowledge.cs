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
        private Process process;
        private ObservableCollection<KnowledgePair> knowledgePairs = new ObservableCollection<KnowledgePair>();
        private int id;

        public InitialKnowledge()
        {
            _id++;
            this.id = _id;
        }

        public bool IsGlobal { get => process == null; } // 是否是全局的，这里直接从Process是否为空计算而来
        public Process Process
        {
            get => process;
            set
            {
                this.RaiseAndSetIfChanged(ref process, value);
                this.RaisePropertyChanged("IsGlobal");
            }
        }
        public ObservableCollection<KnowledgePair> KnowledgePairs { get => knowledgePairs; set => knowledgePairs = value; }
        public int Id
        {
            get => id;
            set
            {
                id = value;
                if (value > _id)
                    _id = value;
            }
        }
    }
}
