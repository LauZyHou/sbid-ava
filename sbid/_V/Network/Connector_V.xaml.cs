using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using sbid._VM;

namespace sbid._V
{
    public class Connector_V : UserControl
    {
        public Connector_V()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        // 鼠标按下
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            // 变色 todo 每类锚点可以继承这个锚点,然后再去做连线行为的处理
            if (ConnectorVM.Color == Brushes.White)
                ConnectorVM.Color = Brushes.Red;
            else
                ConnectorVM.Color = Brushes.White;

            ResourceManager.mainWindowVM.Tips = ConnectorVM.Color.ToString();

            e.Handled = true;
        }

        public Connector_VM ConnectorVM { get => (Connector_VM)DataContext; }
    }
}
