using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using sbid._M;
using sbid._VM;
using System;
using System.Collections.Generic;

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
            /*
            this.get_control_reference();
            this.get_process_names();
            this.init_evnet();
            this.draw_all();
            */
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #region Canvas绘图

        // 绘制全部图形 todo 根据实际执行数据
        private void draw_all()
        {
            // 进程数量
            int processNum = processNames.Count;
            // 实际可用宽高带来的偏置
            double deltaX = CanvasWidth / 12;
            double deltaY = CanvasHeight / 8;
            // 根据进程数量绘制竖线
            if (processNum == 0)
            {
                return;
            }
            if (processNum == 1)
            {
                // 只有一条竖线时居中
                draw_line_vertical(CanvasWidth / 2, deltaY, CanvasHeight - deltaY);
                draw_text(processNames[0], CanvasWidth / 2, deltaY);
                // 记录这个竖线的横向位置
                processStartXs.Clear();
                processStartXs.Add(CanvasWidth / 2);
            }
            else
            {
                // 计算横向竖线间隔，然后依次绘制
                // 注意，绘制时要记录startX的位置
                double gapX = (CanvasWidth - 2 * deltaX) / (processNum - 1);
                processStartXs.Clear();
                for (int i = 0; i < processNum; i++)
                {
                    double startX = deltaX + gapX * i;
                    double startY = deltaY;
                    double endY = CanvasHeight - deltaY;
                    draw_line_vertical(startX, startY, endY);
                    draw_text(processNames[i], startX, startY);
                    // 记录这个竖线的横向位置
                    processStartXs.Add(startX);
                }
                // TODO 删掉下面这行测试内容
                draw_arrow_horizontal(400, processStartXs[0], processStartXs[processStartXs.Count - 1]);
            }
        }

        // 画竖直线
        private void draw_line_vertical(double X, double startY, double endY)
        {
            draw_line(X, startY, X, endY);
        }

        // 画横箭头
        private void draw_arrow_horizontal(double Y, double startX, double endX)
        {
            draw_line(startX, Y, endX, Y);
            // 判断箭头是向左还是向右的
            if (startX > endX) // 向左
            {
                draw_line(endX, Y, endX + 10, Y + 10);
                draw_line(endX, Y, endX + 10, Y - 10);
            }
            else // 向右
            {
                draw_line(endX, Y, endX - 10, Y + 10);
                draw_line(endX, Y, endX - 10, Y - 10);
            }
        }

        // 画直线
        private void draw_line(double startX, double startY, double endX, double endY)
        {
            LineGeometry myLineGeometry = new LineGeometry();
            myLineGeometry.StartPoint = new Point(startX, startY);
            myLineGeometry.EndPoint = new Point(endX, endY);

            Path myPath = new Path();
            myPath.Stroke = Brushes.Black;
            myPath.StrokeThickness = 1;
            myPath.Data = myLineGeometry;

            canvas.Children.Add(myPath);
        }

        // 写文本，在这里自动向左上角偏移
        private void draw_text(string text, double x, double y, int fontSize = 22)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.FontSize = fontSize;
            textBlock.Background = Brushes.AntiqueWhite;

            // 计算文字占用的横向距离，以用来将其对齐中间位置
            double textWidth = 0;
            foreach (char c in text)
            {
                if (c > 127) // 非ASCII字（如中文字）加完整
                    textWidth += fontSize;
                else if (c >= 'A' && c <= 'Z') // 大写字母
                    textWidth += fontSize * 2.0 / 3;
                else if (c >= 'a' && c <= 'z') // 小写字母
                    textWidth += fontSize * 9.0 / 16;
                else // 其它的
                    textWidth += fontSize * 10.0 / 16;
            }

            // 向左上角偏移量
            double dx = textWidth / 2;
            double dy = fontSize / 2;

            Canvas.SetLeft(textBlock, x - dx);
            Canvas.SetTop(textBlock, y - dy);
            canvas.Children.Add(textBlock);
        }

        // 清除所有图形
        private void clear_all()
        {
            canvas.Children.Clear();
        }

        #endregion

        #region 初始化

        // 事件处理
        private void init_evnet()
        {
            // 在Canvas面板的Bounds的宽(Width)高(Height)变化时重新绘制
            var observable = canvas.GetObservable(Canvas.BoundsProperty);
            observable.Subscribe(value =>
            {
                this.clear_all();
                this.draw_all();
            });
        }


        #endregion

        #region 资源引用

        private Canvas canvas;

        // 获取控件引用
        private void get_control_reference()
        {
            canvas = ControlExtensions.FindControl<Canvas>(this, nameof(canvas));
        }

        public SimulationRun_SW_VM VM { get => (SimulationRun_SW_VM)DataContext; }

        public double CanvasWidth { get => canvas.Bounds.Width; }
        public double CanvasHeight { get => canvas.Bounds.Height; }

        // 存放所有进程的名字
        private List<string> processNames = new List<string>();
        // 记录进程的startX位置
        private List<double> processStartXs = new List<double>();

        // 获取当前协议下所有的进程模板名字
        private void get_process_names()
        {
            Protocol_VM protocol_VM = ResourceManager.mainWindowVM.SelectedItem;
            if (protocol_VM is null)
            {
                return;
            }
            ClassDiagram_P_VM classDiagram_P_VM = (ClassDiagram_P_VM)protocol_VM.PanelVMs[0].SidePanelVMs[0];
            foreach (ViewModelBase vmb in classDiagram_P_VM.UserControlVMs)
            {
                if (vmb is Process_VM)
                {
                    Process_VM process_VM = (Process_VM)vmb;
                    processNames.Add(process_VM.Process.RefName.Content);
                }
            }
        }

        #endregion
    }
}
