﻿using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    // 可以 移动/选中 的结点
    public class NetworkNode : ViewModelBase
    {
        private int x;
        private int y;

        public int X { get => x; set => this.RaiseAndSetIfChanged(ref x, value); }
        public int Y { get => y; set => this.RaiseAndSetIfChanged(ref y, value); }
    }
}
