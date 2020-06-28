using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // 拓扑图边上的内容
    public class TopoEdge : ReactiveObject
    {
        private string cost = "0";
        private CommMethodPair commMethodPair;

        // 通信代价，应该是一个数，不过这里只要让用户写字符串就行了
        public string Cost { get => cost; set => this.RaiseAndSetIfChanged(ref cost, value); }
        // 通信方法对
        public CommMethodPair CommMethodPair
        {
            get => commMethodPair;
            set
            {
                this.RaiseAndSetIfChanged(ref commMethodPair, value);
                this.RaisePropertyChanged(nameof(NullCommMethodPair));
            }
        }

        public bool NullCommMethodPair { get => commMethodPair == null; }
    }
}
