using Avalonia;
using sbid._M;
using sbid._V;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    // 顺序图三类消息传递的基类VM
    public abstract class Message_VM : Arrow_VM
    {
        private Formula message;

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

        #region 右键菜单命令

        // 尝试打开编辑消息内容的窗口
        public void EditMessage()
        {
            // 从主窗体打开编辑窗体,并在其DataContext中集成当前Message_VM里集成的Message对象,以能对其作修改
            Message_EW_V messageEWV = new Message_EW_V()
            {
                DataContext = new Message_EW_VM()
                {
                    Message = message
                }
            };
            messageEWV.ShowDialog(ResourceManager.mainWindowV);
            ResourceManager.mainWindowVM.Tips = "打开了消息编辑窗体";
        }

        #endregion
    }
}
