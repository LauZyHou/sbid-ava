using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using sbid._M;
using sbid._VM;
using System.Collections.ObjectModel;

namespace sbid._V
{
    public class CommChannel_EW_V : Window
    {
        #region 构造

        public CommChannel_EW_V()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            this.get_control_reference();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #endregion

        #region 按钮命令

        public void Add_CommMethodPair()
        {
            if (processA_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定发送方进程模板！";
                return;
            }
            Process processA = (Process)processA_ComboBox.SelectedItem;

            if (processB_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定接收方进程模板！";
                return;
            }
            Process processB = (Process)processB_ComboBox.SelectedItem;

            if (commMethodA_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定发送方法！";
                return;
            }
            CommMethod commMethodA = (CommMethod)commMethodA_ComboBox.SelectedItem;
            if (commMethodA.InOutSuffix != InOut.Out)
            {
                ResourceManager.mainWindowVM.Tips = "发送方法必须是Out！";
                return;
            }

            if (commMethodB_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定接收方法！";
                return;
            }
            CommMethod commMethodB = (CommMethod)commMethodB_ComboBox.SelectedItem;
            if (commMethodB.InOutSuffix != InOut.In)
            {
                ResourceManager.mainWindowVM.Tips = "接收方法必须是In！";
                return;
            }

            bool privacy = privacy_CheckBox.IsChecked == null ? false : (bool)privacy_CheckBox.IsChecked;

            CommMethodPair commMethodPair = new CommMethodPair(processA, commMethodA, processB, commMethodB, privacy);
            VM.CommChannel.CommMethodPairs.Add(commMethodPair);
            ResourceManager.mainWindowVM.Tips = "已添加成员：" + commMethodPair;
        }

        public void Update_CommMethodPair()
        {
            if (commMethodPair_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要在域内成员列表中选定要修改的成员！";
                return;
            }

            if (processA_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定发送方进程模板！";
                return;
            }
            Process processA = (Process)processA_ComboBox.SelectedItem;

            if (processB_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定接收方进程模板！";
                return;
            }
            Process processB = (Process)processB_ComboBox.SelectedItem;

            if (commMethodA_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定发送方法！";
                return;
            }
            CommMethod commMethodA = (CommMethod)commMethodA_ComboBox.SelectedItem;
            if (commMethodA.InOutSuffix != InOut.Out)
            {
                ResourceManager.mainWindowVM.Tips = "发送方法必须是Out！";
                return;
            }

            if (commMethodB_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定接收方法！";
                return;
            }
            CommMethod commMethodB = (CommMethod)commMethodB_ComboBox.SelectedItem;
            if (commMethodB.InOutSuffix != InOut.In)
            {
                ResourceManager.mainWindowVM.Tips = "接收方法必须是In！";
                return;
            }

            bool privacy = privacy_CheckBox.IsChecked == null ? false : (bool)privacy_CheckBox.IsChecked;

            CommMethodPair commMethodPair = (CommMethodPair)commMethodPair_ListBox.SelectedItem;
            commMethodPair.ProcessA = processA;
            commMethodPair.CommMethodA = commMethodA;
            commMethodPair.ProcessB = processB;
            commMethodPair.CommMethodB = commMethodB;
            commMethodPair.Privacy = privacy;
            ResourceManager.mainWindowVM.Tips = "已更新成员：" + commMethodPair;
        }

        public void Delete_CommMethodPair()
        {
            if (commMethodPair_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要在域内成员列表中选定要删除的成员！";
                return;
            }

            CommMethodPair commMethodPair = (CommMethodPair)commMethodPair_ListBox.SelectedItem;
            VM.CommChannel.CommMethodPairs.Remove(commMethodPair);
            ResourceManager.mainWindowVM.Tips = "已删除成员：" + commMethodPair;
        }

        #endregion

        #region 资源引用

        private ComboBox processA_ComboBox, processB_ComboBox, commMethodA_ComboBox, commMethodB_ComboBox;
        private ListBox commMethodPair_ListBox;
        private CheckBox privacy_CheckBox;

        // 获取控件引用
        private void get_control_reference()
        {
            processA_ComboBox = ControlExtensions.FindControl<ComboBox>(this, nameof(processA_ComboBox));
            processB_ComboBox = ControlExtensions.FindControl<ComboBox>(this, nameof(processB_ComboBox));
            commMethodA_ComboBox = ControlExtensions.FindControl<ComboBox>(this, nameof(commMethodA_ComboBox));
            commMethodB_ComboBox = ControlExtensions.FindControl<ComboBox>(this, nameof(commMethodB_ComboBox));
            commMethodPair_ListBox = ControlExtensions.FindControl<ListBox>(this, nameof(commMethodPair_ListBox));
            privacy_CheckBox = ControlExtensions.FindControl<CheckBox>(this, nameof(privacy_CheckBox));
            // 另外在xaml里设置IsThreeState="False"，这仅仅表示不允许用户在界面上设置null值
            // https://docs.microsoft.com/zh-cn/dotnet/api/system.windows.controls.primitives.togglebutton.ischecked
            // 所以这里刚创建时候将其设置为false，那用户在使用过程中就不会搞出null了
            privacy_CheckBox.IsChecked = false;
        }

        public CommChannel_EW_VM VM { get => ((CommChannel_EW_VM)DataContext); }

        #endregion
    }
}
