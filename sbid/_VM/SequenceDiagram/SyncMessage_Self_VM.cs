using Avalonia;
using sbid._M;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    // 到自己的同步消息
    public class SyncMessage_Self_VM : Message_VM
    {
        public SyncMessage_Self_VM()
        {
            Message = new Formula("同步消息(到自己)");
        }

        // 消息所在的位置点,位于两锚点中心附近,这里new关键字是有意隐藏父类的MidPos属性
        public new Point MidPos
        {
            get
            {
                double x = (Source.Pos.X + Dest.Pos.X) / 2;
                double y = (Source.Pos.Y + Dest.Pos.Y) / 2;
                return new Point(x + 20, y - 10); // 目的是为了修改这里返回的X坐标
            }
        }
    }
}
