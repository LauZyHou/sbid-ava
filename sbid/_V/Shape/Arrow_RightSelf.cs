using Avalonia;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._V
{
    // 从右侧回到自己的箭头
    public class Arrow_RightSelf : Arrow
    {
        private double rightBias = 50;

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

                // 往右走一部分的点
                double X1Right = X1 + rightBias;
                double X2Right = X2 + rightBias;

                //(X1,Y1)--------(X1Right,Y1)
                //                     |
                //                     |
                //                     |
                //(X2,Y2)<-------(X2Right,Y2)

                // 这里要计算的角就是下侧的边了
                double theta = Math.Atan2(Y2 - Y2, X2Right - X2);
                double sint = Math.Sin(theta);
                double cost = Math.Cos(theta);

                Point pt1 = new Point(X1, Y1);
                Point pt2 = new Point(X1Right, Y1);
                Point pt3 = new Point(X2Right, Y2);
                Point pt4 = new Point(X2, Y2);

                double HeadWidth = this.HeadWidth;
                double HeadHeight = this.HeadHeight;

                // 箭帽的两个点
                Point pt5 = new Point(
                    X2 + (HeadWidth * cost - HeadHeight * sint),
                    Y2 + (HeadWidth * sint + HeadHeight * cost));
                Point pt6 = new Point(
                    X2 + (HeadWidth * cost + HeadHeight * sint),
                    Y2 - (HeadHeight * cost - HeadWidth * sint));

                context.BeginFigure(pt1, false);
                context.LineTo(pt2);
                context.LineTo(pt3);
                context.LineTo(pt4);
                context.EndFigure(false);
                context.BeginFigure(pt5, false);
                context.LineTo(pt4);
                context.LineTo(pt6);
                context.EndFigure(false);
            }
            return streamGeometry;
        }

        // 往右偏移的偏移量
        public double RightBias { get => rightBias; set => rightBias = value; }
    }
}
