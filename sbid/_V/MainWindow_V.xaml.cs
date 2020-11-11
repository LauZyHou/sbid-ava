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
            this.init_binding();
            // 把自己挂到全局资源上
            ResourceManager.mainWindowV = this;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #region 私有

        // 初始化.cs文件中的数据绑定,一些不方便在xaml中绑定的部分在这里绑定
        private void init_binding()
        {
            // 绑定ConnectorVisible
            /*
            ComboBox connectorVisible_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "connectorVisible_ComboBox");
            List<bool> boolList = new List<bool>();
            boolList.Add(true);
            boolList.Add(false);
            connectorVisible_ComboBox.Items = boolList;
            */
            CheckBox connectorVisible_CheckBox = ControlExtensions.FindControl<CheckBox>(this, nameof(connectorVisible_CheckBox));
            connectorVisible_CheckBox.IsChecked = false;
        }

        #endregion
    }
}
