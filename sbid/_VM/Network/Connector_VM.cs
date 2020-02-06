using Avalonia;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    // 锚点VM
    public class Connector_VM : ViewModelBase
    {
        private double x;
        private double y;
        private Point pos;

        // 锚点位置(fixme 改成中心位置?)
        public double X
        {
            get => x;
            set
            {
                this.RaiseAndSetIfChanged(ref x, value);
                Pos = new Point(x, y);
            }
        }

        public double Y
        {
            get => y;
            set
            {
                this.RaiseAndSetIfChanged(ref y, value);
                Pos = new Point(x, y);
            }
        }

        // 在X,Y变化时通知Pos
        public Point Pos { get => pos; set => this.RaiseAndSetIfChanged(ref pos, value); }
    }
}
