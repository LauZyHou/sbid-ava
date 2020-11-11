using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace sbid._V
{
    public class ErrorBox_SW_V : Window
    {
        public ErrorBox_SW_V()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
