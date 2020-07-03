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

        /*
        public void Add_Guard()
        {
            if (guard_TextBox.Text == null || guard_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要给出转移条件！";
                return;
            }

            Formula guard = new Formula(guard_TextBox.Text);
            VM.StateTrans.Guards.Add(guard);
            ResourceManager.mainWindowVM.Tips = "添加了转移条件：" + guard.Content;
        }

        public void Update_Guard()
        {
            if (guard_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要修改的转移条件！";
                return;
            }

            if (guard_TextBox.Text == null || guard_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要给出转移条件！";
                return;
            }

            Formula guard = (Formula)guard_ListBox.SelectedItem;
            guard.Content = guard_TextBox.Text;
            ResourceManager.mainWindowVM.Tips = "修改了转移条件：" + guard.Content;
        }

        public void Delete_Guard()
        {
            if (guard_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要删除的转移条件！";
                return;
            }

            Formula guard = (Formula)guard_ListBox.SelectedItem;
            VM.StateTrans.Guards.Remove(guard);
            ResourceManager.mainWindowVM.Tips = "删除了转移条件：" + guard.Content;
        }
        */

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

        #endregion


        #region 资源引用

        private ListBox guard_ListBox, action_ListBox;
        private TextBox guard_TextBox, action_TextBox;

        // 获取控件引用
        private void get_control_reference()
        {
            // guard_ListBox = ControlExtensions.FindControl<ListBox>(this, "guard_ListBox");
            action_ListBox = ControlExtensions.FindControl<ListBox>(this, "action_ListBox");
            // guard_TextBox = ControlExtensions.FindControl<TextBox>(this, "guard_TextBox");
            action_TextBox = ControlExtensions.FindControl<TextBox>(this, "action_TextBox");
        }

        public StateTrans_EW_VM VM { get => (StateTrans_EW_VM)DataContext; }

        #endregion
    }
}
