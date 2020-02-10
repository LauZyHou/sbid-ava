using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace sbid._V
{
    public class InitialKnowledge_V : NetworkItem_V
    {
        public InitialKnowledge_V()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
