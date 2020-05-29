using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using sbid._M;

namespace sbid._V
{
    public class CTLRelation_EW_V : Window
    {
        public CTLRelation_EW_V()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            init_binding();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #region 辅助构造

        private void init_binding()
        {
            ComboBox ctlRelation_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "ctlRelation_ComboBox");
            ctlRelation_ComboBox.Items = System.Enum.GetValues(typeof(CTLRelation));
        }

        #endregion
    }
}
