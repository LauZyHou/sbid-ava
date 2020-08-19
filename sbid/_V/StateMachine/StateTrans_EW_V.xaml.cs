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
            this.get_control_reference();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #region 按钮命令（核心功能）

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

        #region 按钮命令（辅助输入）
        
        // 将属性接入
        private void Append_PropNav()
        {
            if (propNav_TreeView.SelectedItem is null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要接入的属性！";
                return;
            }

            Nav nav = (Nav)propNav_TreeView.SelectedItem;
            string navStr = nav.Identifier + (nav.IsArray ? ("[" + nav.ArrayIndex + "]") : "");
            while (nav.ParentNav != null)
            {
                nav = nav.ParentNav;
                navStr = nav.Identifier + (nav.IsArray ? ("[" + nav.ArrayIndex + "]") : "") + "." + navStr;
            }

            Append_Symbol(navStr);
        }

        // 将类型名接入
        private void Append_TypeNav()
        {
            if (type_ComboBox.SelectedItem is null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要接入的类型！";
                return;
            }
            Type type = (Type)type_ComboBox.SelectedItem;
            Append_Symbol(type.Name);
        }

        // 将方法调用接入
        private void Append_MethodNav()
        {
            if (method_ComboBox.SelectedItem is null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要接入的方法！";
                return;
            }
            Method method = (Method)method_ComboBox.SelectedItem;
            // 生成方法调用的提示字符串并接入
            int paramNum = method.Parameters.Count;
            string methodCallStr = method.Name + "(";
            if (paramNum == 0)
            {
                methodCallStr += ")";
            }
            else
            {
                for (int i = 0; i < paramNum - 1; i++)
                {
                    methodCallStr += "?, ";
                }
                methodCallStr += "?)";
            }
            Append_Symbol(methodCallStr);
        }

        // 将通信方法调用接入
        private void Append_CommMethodNav()
        {
            if (commMethod_ComboBox.SelectedItem is null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要接入的通信方法！";
                return;
            }
            CommMethod commMethod = (CommMethod)commMethod_ComboBox.SelectedItem;
            // 生成通信方法调用的提示字符串并接入
            int paramNum = commMethod.Parameters.Count;
            string commMethodCallStr = commMethod.Name + "(";
            if (paramNum == 0)
            {
                commMethodCallStr += ")";
            }
            else
            {
                for (int i = 0; i < paramNum - 1; i++)
                {
                    commMethodCallStr += "?, ";
                }
                commMethodCallStr += "?)";
            }
            Append_Symbol(commMethodCallStr);
        }

        // 通用：向光标所在的TextBox中插入字符串symbol
        // 在光标所在位置插入symbol，如果选中了一段，将这段替换为插入的symbol
        private void Append_Symbol(string symbol)
        {
            if (guard_TextBox.Text == null)
            {
                guard_TextBox.Text = "";
            }
            if (action_TextBox.Text == null)
            {
                action_TextBox.Text = "";
            }
            // 判断选中的是哪个TextBox
            TextBox textBox = null;
            if (guard_TextBox.IsFocused)
            {
                textBox = guard_TextBox;
            }
            else if (action_TextBox.IsFocused)
            {
                textBox = action_TextBox;
            }
            else
            {
                ResourceManager.mainWindowVM.Tips = "需要在要接入的位置点下光标！";
                return;
            }
            int start = textBox.SelectionStart;
            int end = textBox.SelectionEnd;
            // 可能会鼠标从右往左选择，所以要判断交换两者的值，保证start在左边
            if (end < start)
            {
                int tmp = start;
                start = end;
                end = tmp;
            }
            string leftStr = textBox.Text.Substring(0, start);
            string rightStr = textBox.Text.Substring(end);
            textBox.Text = leftStr + symbol + rightStr;
            textBox.SelectionStart = textBox.SelectionEnd = start + symbol.Length;
            ResourceManager.mainWindowVM.Tips = "接入了：" + symbol;
        }

        #endregion

        #region 资源引用

        private ListBox action_ListBox;
        private TextBox action_TextBox, guard_TextBox;
        private ComboBox type_ComboBox, method_ComboBox, commMethod_ComboBox;
        private TreeView propNav_TreeView;

        // 获取控件引用
        private void get_control_reference()
        {
            action_ListBox = ControlExtensions.FindControl<ListBox>(this, nameof(action_ListBox));
            action_TextBox = ControlExtensions.FindControl<TextBox>(this, nameof(action_TextBox));
            guard_TextBox = ControlExtensions.FindControl<TextBox>(this, nameof(guard_TextBox));
            type_ComboBox = ControlExtensions.FindControl<ComboBox>(this, nameof(type_ComboBox));
            method_ComboBox = ControlExtensions.FindControl<ComboBox>(this, nameof(method_ComboBox));
            commMethod_ComboBox = ControlExtensions.FindControl<ComboBox>(this, nameof(commMethod_ComboBox));
            propNav_TreeView = ControlExtensions.FindControl<TreeView>(this, nameof(propNav_TreeView));
        }

        public StateTrans_EW_VM VM { get => (StateTrans_EW_VM)DataContext; }

        #endregion
    }
}
