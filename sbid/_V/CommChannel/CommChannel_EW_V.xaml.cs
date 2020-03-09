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
            ListBox process_ListBox = ControlExtensions.FindControl<ListBox>(this, "process_ListBox");
            if (process_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定进程模板！";
                return;
            }

            ListBox commMethod_ListBox = ControlExtensions.FindControl<ListBox>(this, "commMethod_ListBox");
            if (commMethod_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定通信方法！";
                return;
            }

            CommMethodPair commMethodPair = new CommMethodPair((Process)process_ListBox.SelectedItem, (CommMethod)commMethod_ListBox.SelectedItem);
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

            ListBox process_ListBox = ControlExtensions.FindControl<ListBox>(this, "process_ListBox");
            if (process_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定进程模板！";
                return;
            }

            ListBox commMethod_ListBox = ControlExtensions.FindControl<ListBox>(this, "commMethod_ListBox");
            if (commMethod_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定通信方法！";
                return;
            }

            CommMethodPair commMethodPair = (CommMethodPair)commMethodPair_ListBox.SelectedItem;
            commMethodPair.Process = (Process)process_ListBox.SelectedItem;
            commMethodPair.CommMethod = (CommMethod)commMethod_ListBox.SelectedItem;
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
