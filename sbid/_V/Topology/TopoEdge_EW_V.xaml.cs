using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using sbid._VM;
using System.ComponentModel;

namespace sbid._V
{
    public class TopoEdge_EW_V : Window
    {
        public TopoEdge_EW_V()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            this.Closing += close_event;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #region 事件

        // 关闭窗体
        private void close_event(object sender, CancelEventArgs e)
        {
            TopoGraph_P_VM topoGraph_P_VM = (TopoGraph_P_VM)ResourceManager.mainWindowVM.SelectedItem.SelectedItem.SelectedItem;
            topoGraph_P_VM.PanelEnabled = true;
        }

        #endregion

        #region 按钮命令

        #endregion

        #region 资源引用

        #endregion
    }
}
