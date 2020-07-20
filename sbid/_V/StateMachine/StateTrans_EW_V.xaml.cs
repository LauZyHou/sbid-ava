using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using sbid._M;
using sbid._VM;

namespace sbid._V
{
    public class StateTrans_EW_V : Window
    {
        public StateTrans_EW_V()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            // 获取控件引用
            this.get_control_reference();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #region 按钮命令

        public void Add_Action()
        {
            if (action_TextBox.Text == null || action_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要给出转移动作！";
                return;
            }

            Formula action = new Formula(action_TextBox.Text);
            VM.StateTrans.Actions.Add(action);
            ResourceManager.mainWindowVM.Tips = "添加了转移动作：" + action.Content;
        }

        public void Update_Action()
        {
            if (action_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要修改的转移动作！";
                return;
            }

            if (action_TextBox.Text == null || action_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要给出转移动作！";
                return;
            }

            Formula action = (Formula)action_ListBox.SelectedItem;
            action.Content = action_TextBox.Text;
            ResourceManager.mainWindowVM.Tips = "修改了转移动作：" + action.Content;
        }

        public void Delete_Action()
        {
            if (action_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要删除的转移动作！";
                return;
            }

            Formula action = (Formula)action_ListBox.SelectedItem;
            VM.StateTrans.Actions.Remove(action);
            ResourceManager.mainWindowVM.Tips = "删除了转移动作：" + action.Content;
        }

        public void Move_Up()
        {
            if (action_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要上移的转移动作！";
                return;
            }

            if (action_ListBox.SelectedIndex == 0)
            {
                ResourceManager.mainWindowVM.Tips = "已经是第一个了！";
                return;
            }

            Formula action = (Formula)action_ListBox.SelectedItem;
            int newIndex = VM.StateTrans.Actions.IndexOf(action) - 1;
            VM.StateTrans.Actions.Remove(action);
            VM.StateTrans.Actions.Insert(newIndex, action);
            ResourceManager.mainWindowVM.Tips = "上移了转移动作：" + action.Content;
            action_ListBox.SelectedItem = action;
        }

        public void Move_Down()
        {
            if (action_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要下移的转移动作！";
                return;
            }

            if (action_ListBox.SelectedIndex == VM.StateTrans.Actions.Count - 1)
            {
                ResourceManager.mainWindowVM.Tips = "已经是最后一个了！";
                return;
            }

            Formula action = (Formula)action_ListBox.SelectedItem;
            int newIndex = VM.StateTrans.Actions.IndexOf(action) + 1;
            VM.StateTrans.Actions.Remove(action);
            VM.StateTrans.Actions.Insert(newIndex, action);
            ResourceManager.mainWindowVM.Tips = "下移了转移动作：" + action.Content;
            action_ListBox.SelectedItem = action;
        }

        #endregion


        #region 资源引用

        private ListBox action_ListBox;
        private TextBox action_TextBox;

        // 获取控件引用
        private void get_control_reference()
        {
            action_ListBox = ControlExtensions.FindControl<ListBox>(this, "action_ListBox");
            action_TextBox = ControlExtensions.FindControl<TextBox>(this, "action_TextBox");
        }

        public StateTrans_EW_VM VM { get => (StateTrans_EW_VM)DataContext; }

        #endregion
    }
}
