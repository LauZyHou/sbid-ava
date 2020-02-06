using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace sbid._V
{
    public class MainWindow_V : Window
    {
        public MainWindow_V()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            // 把自己挂到全局资源上
            ResourceManager.mainWindowV = this;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
