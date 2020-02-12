using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._V
{
    // 带三角箭头的直线图形
    public class TriangleArrow : Arrow
    {
        // 在此覆写方法中实现图形
        protected override Geometry CreateDefiningGeometry()
        {
            StreamGeometry streamGeometry = new StreamGeometry();
            using (StreamGeometryContext context = streamGeometry.Open())
            {
                double X1 = StartPoint.X;
                double X2 = EndPoint.X;
                double Y1 = StartPoint.Y;
                double Y2 = EndPoint.Y;

                double theta = Math.Atan2(Y1 - Y2, X1 - X2);
                double sint = Math.Sin(theta);
                double cost = Math.Cos(theta);

                Point pt1 = new Point(X1, Y1);
                Point pt2 = new Point(X2, Y2);

                double HeadWidth = this.HeadWidth;
                double HeadHeight = this.HeadHeight;

                Point pt3 = new Point(
                    X2 + (HeadWidth * cost - HeadHeight * sint),
                    Y2 + (HeadWidth * sint + HeadHeight * cost));

                Point pt4 = new Point(
                    X2 + (HeadWidth * cost + HeadHeight * sint),
                    Y2 - (HeadHeight * cost - HeadWidth * sint));

                // 绘制直线
                context.BeginFigure(pt1, false);
                context.LineTo(pt2);
                context.EndFigure(false);
                // 绘制封闭三角
                context.BeginFigure(pt2, true);
                context.LineTo(pt3);
                context.LineTo(pt4);
                context.EndFigure(true);
                Fill = Brushes.Black;
            }
            return streamGeometry;
        }
    }
}
