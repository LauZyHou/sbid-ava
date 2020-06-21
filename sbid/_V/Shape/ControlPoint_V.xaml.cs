using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace sbid._V
{
    public class ControlPoint_V : NetworkItem_V
    {
        public ControlPoint_V()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
