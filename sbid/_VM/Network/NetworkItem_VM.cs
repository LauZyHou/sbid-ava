using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    // 可以 移动/选中 的结点
    public class NetworkItem_VM : ViewModelBase
    {
        private double x;
        private double y;

        public double X { get => x; set => this.RaiseAndSetIfChanged(ref x, value); }
        public double Y { get => y; set => this.RaiseAndSetIfChanged(ref y, value); }
    }
}
