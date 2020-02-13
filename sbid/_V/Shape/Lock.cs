using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._V
{
    public class Lock : Rectangle
    {
        // 在此覆写方法中实现图形
        protected override Geometry CreateDefiningGeometry()
        {
            StreamGeometry streamGeometry = new StreamGeometry();
            using (StreamGeometryContext context = streamGeometry.Open())
            {
                //  -- 
                // |  |
                //------
                //|    |
                //------

                // 画锁圈
                Point pt1 = new Point(Width / 4, Height / 2);
                Point pt2 = new Point(Width / 4, 0);
                Point pt3 = new Point(Width / 4 * 3, 0);
                Point pt4 = new Point(Width / 4 * 3, Height / 2);
                context.BeginFigure(pt1, false);
                context.LineTo(pt2);
                context.LineTo(pt3);
                context.LineTo(pt4);
                context.EndFigure(false);

                // 画锁体
                Point pt5 = new Point(0, Height / 2);
                Point pt6 = new Point(Width, Height / 2);
                Point pt7 = new Point(Width, Height);
                Point pt8 = new Point(0, Height);
                context.BeginFigure(pt5, false);
                context.LineTo(pt6);
                context.LineTo(pt7);
                context.LineTo(pt8);
                context.EndFigure(true);

                // 画锁上X
                //context.BeginFigure(pt5, false);
                //context.LineTo(pt7);
                //context.EndFigure(false);
                //context.BeginFigure(pt6, false);
                //context.LineTo(pt8);
                //context.EndFigure(false);
            }
            return streamGeometry;
        }
    }
}
