using Avalonia;
using Avalonia.Media;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    // 锚点VM
    public class Connector_VM : ViewModelBase
    {
        private Point pos;
        private Point oldPos;
        private ISolidColorBrush color = Brushes.White;

        // 无参构造用于xaml里Design
        public Connector_VM() { }

        // 带位置的构造
        public Connector_VM(double x, double y)
        {
            Pos = new Point(x, y);
        }

        // 锚点位置
        public Point Pos { get => pos; set => this.RaiseAndSetIfChanged(ref pos, value); }

        // 锚点旧位置,用于在拖拽图形按下时记录,以保证连线跟着变化
        public Point OldPos { get => oldPos; set => oldPos = value; }

        // 锚点颜色
        public ISolidColorBrush Color { get => color; set => this.RaiseAndSetIfChanged(ref color, value); }
    }
}
