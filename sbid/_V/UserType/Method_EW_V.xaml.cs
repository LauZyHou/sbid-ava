using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace sbid._V
{
    public class Method_EW_V : Window
    {
        public Method_EW_V()
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
