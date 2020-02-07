using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
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

        public Connector_VM ConnectorVM { get => (Connector_VM)DataContext; }
    }
}
