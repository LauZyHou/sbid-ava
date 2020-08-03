using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace sbid._V
{
    public class PropertyVerify_SW_V : Window
    {
        public PropertyVerify_SW_V()
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
