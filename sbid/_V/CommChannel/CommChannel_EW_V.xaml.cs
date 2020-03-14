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
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #endregion

        #region 按钮命令

        public void Add_CommMethodPair()
        {
            ComboBox processA_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "processA_ComboBox");
            if (processA_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定发送方进程模板！";
                return;
            }
            Process processA = (Process)processA_ComboBox.SelectedItem;

            ComboBox processB_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "processB_ComboBox");
            if (processB_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定接收方进程模板！";
                return;
            }
            Process processB = (Process)processB_ComboBox.SelectedItem;

            ComboBox commMethodA_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "commMethodA_ComboBox");
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

            ComboBox commMethodB_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "commMethodB_ComboBox");
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

            CommMethodPair commMethodPair = new CommMethodPair(processA, commMethodA, processB, commMethodB);
            ((CommChannel_EW_VM)DataContext).CommChannel.CommMethodPairs.Add(commMethodPair);
            ResourceManager.mainWindowVM.Tips = "已添加成员：" + commMethodPair;
        }

        public void Update_CommMethodPair()
        {
            ListBox commMethodPair_ListBox = ControlExtensions.FindControl<ListBox>(this, "commMethodPair_ListBox");
            if (commMethodPair_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要在域内成员列表中选定要修改的成员！";
                return;
            }

            ComboBox processA_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "processA_ComboBox");
            if (processA_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定发送方进程模板！";
                return;
            }
            Process processA = (Process)processA_ComboBox.SelectedItem;

            ComboBox processB_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "processB_ComboBox");
            if (processB_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定接收方进程模板！";
                return;
            }
            Process processB = (Process)processB_ComboBox.SelectedItem;

            ComboBox commMethodA_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "commMethodA_ComboBox");
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

            ComboBox commMethodB_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "commMethodB_ComboBox");
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

            CommMethodPair commMethodPair = (CommMethodPair)commMethodPair_ListBox.SelectedItem;
            commMethodPair.ProcessA = processA;
            commMethodPair.CommMethodA = commMethodA;
            commMethodPair.ProcessB = processB;
            commMethodPair.CommMethodB = commMethodB;
            ResourceManager.mainWindowVM.Tips = "已更新成员：" + commMethodPair;
        }

        public void Delete_CommMethodPair()
        {
            ListBox commMethodPair_ListBox = ControlExtensions.FindControl<ListBox>(this, "commMethodPair_ListBox");
            if (commMethodPair_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要在域内成员列表中选定要删除的成员！";
                return;
            }

            CommMethodPair commMethodPair = (CommMethodPair)commMethodPair_ListBox.SelectedItem;
            ((CommChannel_EW_VM)DataContext).CommChannel.CommMethodPairs.Remove(commMethodPair);
            ResourceManager.mainWindowVM.Tips = "已删除成员：" + commMethodPair;
        }

        #endregion
    }
}
