using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace sbid._V
{
    public class SimulationRun_SW_V : Window
    {
        public SimulationRun_SW_V()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            this.get_control_reference();
            this.renderAll();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #region Canvas绘图

        // 绘制全部
        private void renderAll()
        {
            LineGeometry myLineGeometry = new LineGeometry();
            myLineGeometry.StartPoint = new Point(100, 100);
            myLineGeometry.EndPoint = new Point(200, 200);

            Path myPath = new Path();
            myPath.Stroke = Brushes.Black;
            myPath.StrokeThickness = 1;
            myPath.Data = myLineGeometry;

            canvas.Children.Add(myPath);
        }

        #endregion

        #region 资源引用

        private Canvas canvas;

        private void get_control_reference()
        {
            canvas = ControlExtensions.FindControl<Canvas>(this, nameof(canvas));
        }

        #endregion
    }
}
