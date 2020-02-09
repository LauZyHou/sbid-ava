using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using sbid._M;
using sbid._VM;

namespace sbid._V
{
    public class Transition_EW_V : Window
    {
        public Transition_EW_V()
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

        #region 按钮命令

        public void Add_Action()
        {
            TextBox action_TextBox = ControlExtensions.FindControl<TextBox>(this, "action_TextBox");
            if (action_TextBox.Text == null || action_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要给出Action的动作描述！";
                return;
            }

            Action action = new Action(action_TextBox.Text);
            ((Transition_EW_VM)DataContext).Transition.Actions.Add(action);

            // todo 转移上StackPanel位置随Action数量变化

            ResourceManager.mainWindowVM.Tips = "添加了转移动作：" + action.Formula;
        }

        public void Update_Action()
        {
            ListBox action_ListBox = ControlExtensions.FindControl<ListBox>(this, "action_ListBox");
            if (action_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要修改的Action！";
                return;
            }

            TextBox action_TextBox = ControlExtensions.FindControl<TextBox>(this, "action_TextBox");
            if (action_TextBox.Text == null || action_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要给出Action的动作描述！";
                return;
            }

            Action action = (Action)action_ListBox.SelectedItem;
            action.Formula = action_TextBox.Text;
            ResourceManager.mainWindowVM.Tips = "修改了转移动作：" + action.Formula;
        }

        public void Delete_Action()
        {
            ListBox action_ListBox = ControlExtensions.FindControl<ListBox>(this, "action_ListBox");
            if (action_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要删除的Action！";
                return;
            }

            Action action = (Action)action_ListBox.SelectedItem;
            ((Transition_EW_VM)DataContext).Transition.Actions.Remove(action);

            // todo 转移上StackPanel位置随Action数量变化

            ResourceManager.mainWindowVM.Tips = "删除了转移动作：" + action.Formula;
        }

        #endregion
    }
}
