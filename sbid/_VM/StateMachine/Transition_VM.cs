using Avalonia;
using ReactiveUI;
using sbid._M;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class Transition_VM : Arrow_VM
    {
        private Transition transition = new Transition();

        public Transition Transition { get => transition; set => this.RaiseAndSetIfChanged(ref transition, value); }

        // 状态机Gurad条件和Actions的位置点,位于两锚点中心附近
        public Point MidPos
        {
            get
            {
                double x = (Source.Pos.X + Dest.Pos.X) / 2;
                double y = (Source.Pos.Y + Dest.Pos.Y) / 2;
                return new Point(x - 40, y - 30);
            }
        }
    }
}
