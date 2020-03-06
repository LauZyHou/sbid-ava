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
            // 初始化.cs文件中的事件处理
            init_event();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #endregion

        #region 按钮命令

        public void Add_CommMethodPair()
        {
            ComboBox process_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "process_ComboBox");
            if (process_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定进程模板！";
                return;
            }

            ComboBox commMethod_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "commMethod_ComboBox");
            if (commMethod_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定通信方法！";
                return;
            }

            CommMethodPair commMethodPair = new CommMethodPair((Process)process_ComboBox.SelectedItem, (CommMethod)commMethod_ComboBox.SelectedItem);
            ((CommChannel_EW_VM)DataContext).CommMethodPairs.Add(commMethodPair);
            ResourceManager.mainWindowVM.Tips = "已在域内成员中添加临时成员：" + commMethodPair;
        }

        public void Update_CommMethodPair()
        {
            ListBox commMethodPair_ListBox = ControlExtensions.FindControl<ListBox>(this, "commMethodPair_ListBox");
            if (commMethodPair_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要在域内成员列表中选定要修改的成员！";
                return;
            }

            ComboBox process_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "process_ComboBox");
            if (process_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定进程模板！";
                return;
            }

            ComboBox commMethod_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "commMethod_ComboBox");
            if (commMethod_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定通信方法！";
                return;
            }

            CommMethodPair commMethodPair = (CommMethodPair)commMethodPair_ListBox.SelectedItem;
            commMethodPair.Process = (Process)process_ComboBox.SelectedItem;
            commMethodPair.CommMethod = (CommMethod)commMethod_ComboBox.SelectedItem;
            ResourceManager.mainWindowVM.Tips = "已在临时域内成员列表中更新成员：" + commMethodPair;
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
            ((CommChannel_EW_VM)DataContext).CommMethodPairs.Remove(commMethodPair);
            ResourceManager.mainWindowVM.Tips = "已在临时域内成员列表中删除成员：" + commMethodPair;
        }

        public void Add_CommDomain()
        {
            TextBox commDomainName_TextBox = ControlExtensions.FindControl<TextBox>(this, "commDomainName_TextBox");
            if (commDomainName_TextBox.Text == null || commDomainName_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要给出域名/信道名！";
                return;
            }

            ObservableCollection<CommMethodPair> commMethodPairs = ((CommChannel_EW_VM)DataContext).CommMethodPairs;
            if (commMethodPairs.Count == 0)
            {
                ResourceManager.mainWindowVM.Tips = "至少要在域内成员列表中添加一个成员！";
                return;
            }

            CommDomain commDomain = new CommDomain(commDomainName_TextBox.Text, commMethodPairs);
            ((CommChannel_EW_VM)DataContext).CommChannel.CommDomains.Add(commDomain);
            ResourceManager.mainWindowVM.Tips = "添加了通信域：" + commDomain;

            // 添加完成后,要将临时域内成员列表拿掉,这样再向临时参数列表中添加/更新内容也不会影响刚刚添加的列表
            ((CommChannel_EW_VM)DataContext).CommMethodPairs = new ObservableCollection<CommMethodPair>();
        }

        public void Update_CommDomain()
        {
            ListBox commDomain_ListBox = ControlExtensions.FindControl<ListBox>(this, "commDomain_ListBox");
            if (commDomain_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要在CommDomain列表中选定要修改的通信域！";
                return;
            }

            TextBox commDomainName_TextBox = ControlExtensions.FindControl<TextBox>(this, "commDomainName_TextBox");
            if (commDomainName_TextBox.Text == null || commDomainName_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要给出域名/信道名！";
                return;
            }

            ObservableCollection<CommMethodPair> commMethodPairs = ((CommChannel_EW_VM)DataContext).CommMethodPairs;
            if (commMethodPairs.Count == 0)
            {
                ResourceManager.mainWindowVM.Tips = "至少要在域内成员列表中添加一个成员！";
                return;
            }

            CommDomain commDomain = new CommDomain(commDomainName_TextBox.Text, commMethodPairs);
            ((CommChannel_EW_VM)DataContext).CommChannel.CommDomains[commDomain_ListBox.SelectedIndex] = commDomain;
            ResourceManager.mainWindowVM.Tips = "修改了通信域：" + commDomain;

            // 修改完成后,要将临时域内成员列表拿掉,这样再向临时参数列表中添加/更新内容也不会影响刚刚添加的列表
            ((CommChannel_EW_VM)DataContext).CommMethodPairs = new ObservableCollection<CommMethodPair>();
        }

        public void Delete_CommDomain()
        {
            ListBox commDomain_ListBox = ControlExtensions.FindControl<ListBox>(this, "commDomain_ListBox");
            if (commDomain_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要在CommDomain列表中选定要删除的通信域！";
                return;
            }

            CommDomain commDomain = (CommDomain)commDomain_ListBox.SelectedItem;
            ((CommChannel_EW_VM)DataContext).CommChannel.CommDomains.Remove(commDomain);
            ResourceManager.mainWindowVM.Tips = "删除了通信域：" + commDomain;
        }

        #endregion

        #region 事件

        // CommDomain右侧列表选中项变化的处理
        private void commDomain_ListBox_Changed(object sender, SelectionChangedEventArgs e)
        {
            // 将右侧选中项的CommMethodPair列表拷贝到commMethodPair_ListBox绑定的CommMethodPairs里
            ListBox commDomain_ListBox = ControlExtensions.FindControl<ListBox>(this, "commDomain_ListBox");
            ((CommChannel_EW_VM)DataContext).CommMethodPairs = new ObservableCollection<CommMethodPair>();
            foreach (CommMethodPair commMethodPair in ((CommDomain)commDomain_ListBox.SelectedItem).CommMethodPairs)
            {
                ((CommChannel_EW_VM)DataContext).CommMethodPairs.Add(new CommMethodPair(commMethodPair.Process, commMethodPair.CommMethod));
            }
            ResourceManager.mainWindowVM.Tips = "选中了CommDomain，已拷贝其参数列表";
        }

        #endregion

        #region 初始化

        // 初始化.cs文件中的事件处理方法,一些无法在xaml中绑定的部分在这里绑定
        private void init_event()
        {
            // 绑定CommDomain右侧列表选中项变化的处理方法
            ListBox commDomain_ListBox = ControlExtensions.FindControl<ListBox>(this, "commDomain_ListBox");
            commDomain_ListBox.SelectionChanged += commDomain_ListBox_Changed;
        }

        #endregion
    }
}
