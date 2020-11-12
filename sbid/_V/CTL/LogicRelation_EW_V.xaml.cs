using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using sbid._M;
using sbid._VM;
using System.ComponentModel;

namespace sbid._V
{
    public class LogicRelation_EW_V : Window
    {
        public LogicRelation_EW_V()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            this.init_binding();
            this.Closing += close_event;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #region 辅助构造

        private void init_binding()
        {
            ComboBox logicRelation_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "logicRelation_ComboBox");
            logicRelation_ComboBox.Items = System.Enum.GetValues(typeof(LogicRelation));
        }

        // 关闭窗体
        private void close_event(object sender, CancelEventArgs e)
        {
            CTLTree_P_VM ctlTree_P_VM = (CTLTree_P_VM)ResourceManager.mainWindowVM.SelectedItem.SelectedItem.SelectedItem;
            ctlTree_P_VM.PanelEnabled = true;
        }

        #endregion
    }
}
