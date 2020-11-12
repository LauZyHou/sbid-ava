﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using sbid._M;
using sbid._VM;
using System.ComponentModel;

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
            ComboBox ctlRelation_ComboBox = ControlExtensions.FindControl<ComboBox>(this, nameof(ctlRelation_ComboBox));
            ctlRelation_ComboBox.Items = System.Enum.GetValues(typeof(CTLRelation));
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
