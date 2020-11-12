using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using sbid._M;
using sbid._VM;
using System.ComponentModel;

namespace sbid._V
{
    public class ObjLifeLine_EW_V : Window
    {
        public ObjLifeLine_EW_V()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            // 初始化.cs文件中的事件处理
            this.init_event();
            this.Closing += close_event;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #region 事件

        // 进程模板选中项变化的处理
        private void process_ComboBox_Changed(object sender, SelectionChangedEventArgs e)
        {
            // 安全锁锁定时不做任何修改
            if (ObjLifeLineEWVM.SafetyLock)
            {
                return;
            }
            // 获取选中的Process
            ComboBox process_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "process_ComboBox");
            if (process_ComboBox.SelectedItem == null)
            {
                return;
            }
            Process process = (Process)process_ComboBox.SelectedItem;
            // 清空从该ObjLifeLine_VM出发的消息连线上的CommMethod
            foreach (Connector_VM connector_VM in ObjLifeLineEWVM.ObjLifeLine_VM.ConnectorVMs)
            {
                Message_VM message_VM = (Message_VM)connector_VM.ConnectionVM;
                if (message_VM != null && message_VM.Source == connector_VM)
                {
                    message_VM.CommMessage.CommMethod = null;
                }
            }
            // 提示
            ResourceManager.mainWindowVM.Tips = "进程模板被修改为[" + process.RefName + "]，旧的消息通信已清除";
        }

        // 关闭窗体
        private void close_event(object sender, CancelEventArgs e)
        {
            SequenceDiagram_P_VM seqDiagramP_VM = (SequenceDiagram_P_VM)ResourceManager.mainWindowVM.SelectedItem.SelectedItem.SelectedItem;
            seqDiagramP_VM.PanelEnabled = true;
        }

        #endregion

        #region 初始化

        // 初始化.cs文件中的事件处理方法,一些无法在xaml中绑定的部分在这里绑定
        private void init_event()
        {
            // 绑定进程模板ComboBox选中项变化的处理方法
            ComboBox process_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "process_ComboBox");
            process_ComboBox.SelectionChanged += process_ComboBox_Changed;
        }

        #endregion

        // 对应的VM
        public ObjLifeLine_EW_VM ObjLifeLineEWVM { get => (ObjLifeLine_EW_VM)DataContext; }
    }
}
