using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // 拓扑图连线的M
    public class TopoLink : ReactiveObject
    {
        private string content;

        public string Content { get => content; set => this.RaiseAndSetIfChanged(ref content, value); }
    }
}
