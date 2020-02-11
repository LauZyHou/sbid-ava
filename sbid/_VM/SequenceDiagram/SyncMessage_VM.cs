using Avalonia;
using sbid._M;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    // 同步消息箭头的VM
    class SyncMessage_VM : Arrow_VM
    {
        private Formula message = new Formula("待编辑同步消息");

        public Formula Message { get => message; set => message = value; }

        // 消息所在的位置点,位于两锚点中心附近
        public Point MidPos
        {
            get
            {
                double x = (Source.Pos.X + Dest.Pos.X) / 2;
                double y = (Source.Pos.Y + Dest.Pos.Y) / 2;
                return new Point(x - 40, y - 10);
            }
        }
    }
}
