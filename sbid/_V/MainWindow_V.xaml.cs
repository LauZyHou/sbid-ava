using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;

namespace sbid._V
{
    public class MainWindow_V : Window
    {
        public MainWindow_V()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            init_binding();
            // 把自己挂到全局资源上
            ResourceManager.mainWindowV = this;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #region 辅助构造

        // 初始化.cs文件中的数据绑定,一些不方便在xaml中绑定的部分在这里绑定
        private void init_binding()
        {
            // 绑定ConnectorVisible
            ComboBox connectorVisible_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "connectorVisible_ComboBox");
            List<bool> boolList = new List<bool>();
            boolList.Add(true);
            boolList.Add(false);
            connectorVisible_ComboBox.Items = boolList;
        }

        #endregion
    }
}
