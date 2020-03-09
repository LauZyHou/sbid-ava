using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._M
{
    // 通信信道
    public class CommChannel : ReactiveObject
    {
        public static int _id = 0;
        private string name;
        private int id;
        private ObservableCollection<CommMethodPair> commMethodPairs = new ObservableCollection<CommMethodPair>();

        public CommChannel()
        {
            _id++;
            this.id = _id;
            this.name = "CC" + this.id;
        }

        public string Name { get => name; set => this.RaiseAndSetIfChanged(ref name, value); }
        public ObservableCollection<CommMethodPair> CommMethodPairs { get => commMethodPairs; set => commMethodPairs = value; }
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
