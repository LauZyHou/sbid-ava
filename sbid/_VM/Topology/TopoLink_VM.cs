using Avalonia;
using ReactiveUI;
using sbid._M;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    // 拓扑图连线的VM
    public class TopoLink_VM : Arrow_VM
    {
        private TopoLink topoLink;

        public TopoLink TopoLink { get => topoLink; set => this.RaiseAndSetIfChanged(ref topoLink, value); }

        // 连线上的文本的位置点,位于两锚点中心附近
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

        // todo

        #endregion
    }
}
