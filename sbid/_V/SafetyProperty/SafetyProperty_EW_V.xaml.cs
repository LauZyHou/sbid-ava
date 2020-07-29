using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using sbid._M;
using sbid._VM;

namespace sbid._V
{
    public class SafetyProperty_EW_V : Window
    {
        public SafetyProperty_EW_V()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            this.get_control_reference();
            this.init_event();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }



        #region 按钮命令（核心功能）

        public void Add_CTL()
        {
            if (ctl_TextBox.Text == null || ctl_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要给出要添加的CTL公式！";
                return;
            }

            Formula formula = new Formula(ctl_TextBox.Text);
            VM.SafetyProperty.CTLs.Add(formula);
            ResourceManager.mainWindowVM.Tips = "添加了CTL公式：" + formula.Content;
        }

        public void Update_CTL()
        {
            if (ctl_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要修改的CTL公式！";
                return;
            }

            if (ctl_TextBox.Text == null || ctl_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要给出修改后的CTL公式！";
                return;
            }

            Formula formula = (Formula)ctl_ListBox.SelectedItem;
            formula.Content = ctl_TextBox.Text;

            ResourceManager.mainWindowVM.Tips = "修改了CTL公式：" + formula.Content;
        }

        public void Delete_CTL()
        {
            if (ctl_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要删除的CTL公式！";
                return;
            }

            Formula formula = (Formula)ctl_ListBox.SelectedItem;
            VM.SafetyProperty.CTLs.Remove(formula);
            ResourceManager.mainWindowVM.Tips = "删除了CTL公式：" + formula.Content;
        }

        public void Add_Invariant()
        {
            if (invariant_TextBox.Text == null || invariant_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要给出要添加的不变性条件！";
                return;
            }

            Formula invariant = new Formula(invariant_TextBox.Text);
            VM.SafetyProperty.Invariants.Add(invariant);
            ResourceManager.mainWindowVM.Tips = "添加了不变性条件：" + invariant.Content;
        }

        public void Update_Invariant()
        {
            if (invariant_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要修改的不变性条件！";
                return;
            }

            if (invariant_TextBox.Text == null || invariant_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要给出修改后的的不变性条件！";
                return;
            }

            Formula invariant = (Formula)invariant_ListBox.SelectedItem;
            invariant.Content = invariant_TextBox.Text;
            ResourceManager.mainWindowVM.Tips = "修改了不变性条件：" + invariant.Content;
        }

        public void Delete_Invariant()
        {
            if (invariant_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要删除的不变性条件！";
                return;
            }

            Formula invariant = (Formula)invariant_ListBox.SelectedItem;
            VM.SafetyProperty.Invariants.Remove(invariant);
            ResourceManager.mainWindowVM.Tips = "删除了不变性条件：" + invariant.Content;
        }

        #endregion

        #region 按钮命令（帮用户写公式的按钮）

        public void Append_CTL_PropNav()
        {
            if (process_CTL_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定进程模板！";
                return;
            }
            Process process = (Process)process_CTL_ComboBox.SelectedItem;

            TreeView ctlPropNav_TreeView = ControlExtensions.FindControl<TreeView>(this, nameof(ctlPropNav_TreeView));
            if (ctlPropNav_TreeView.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要接入的属性！";
                return;
            }

            Nav nav = (Nav)ctlPropNav_TreeView.SelectedItem;
            string navStr = nav.Identifier + (nav.IsArray ? ("[" + nav.ArrayIndex + "]") : "");
            while (nav.ParentNav != null)
            {
                nav = nav.ParentNav;
                navStr = nav.Identifier + (nav.IsArray ? ("[" + nav.ArrayIndex + "]") : "") + "." + navStr;
            }
            navStr = process.RefName.Content + "." + navStr;

            Append_CTL_Symbol(navStr);
        }

        public void Append_Inv_PropNav()
        {
            if (process_Inv_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定进程模板！";
                return;
            }
            Process process = (Process)process_Inv_ComboBox.SelectedItem;

            TreeView invPropNav_TreeView = ControlExtensions.FindControl<TreeView>(this, nameof(invPropNav_TreeView));
            if (invPropNav_TreeView.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要接入的属性！";
                return;
            }

            Nav nav = (Nav)invPropNav_TreeView.SelectedItem;
            string navStr = nav.Identifier + (nav.IsArray ? ("[" + nav.ArrayIndex + "]") : "");
            while (nav.ParentNav != null)
            {
                nav = nav.ParentNav;
                navStr = nav.Identifier + (nav.IsArray ? ("[" + nav.ArrayIndex + "]") : "") + "." + navStr;
            }
            navStr = process.RefName.Content + "." + navStr;

            Append_Inv_Symbol(navStr);
        }

        #endregion

        #region 私有工具

        // 向ctl_TextBox中插入字符串symbol
        // 在光标所在位置插入symbol，如果选中了一段，将这段替换为插入的symbol
        private void Append_CTL_Symbol(string symbol)
        {
            if (ctl_TextBox.Text == null)
            {
                ctl_TextBox.Text = "";
            }
            int start = ctl_TextBox.SelectionStart;
            int end = ctl_TextBox.SelectionEnd;
            // 可能会鼠标从右往左选择，所以要判断交换两者的值，保证start在左边
            if (end < start)
            {
                int tmp = start;
                start = end;
                end = tmp;
            }
            string leftStr = ctl_TextBox.Text.Substring(0, start);
            string rightStr = ctl_TextBox.Text.Substring(end);
            ctl_TextBox.Text = leftStr + symbol + rightStr;
            ctl_TextBox.SelectionStart = ctl_TextBox.SelectionEnd = start + symbol.Length;
        }

        // 同上，只是针对Invarint的文本框
        private void Append_Inv_Symbol(string symbol)
        {
            if (invariant_TextBox.Text == null)
            {
                invariant_TextBox.Text = "";
            }
            int start = invariant_TextBox.SelectionStart;
            int end = invariant_TextBox.SelectionEnd;
            if (end < start)
            {
                int tmp = start;
                start = end;
                end = tmp;
            }
            string leftStr = invariant_TextBox.Text.Substring(0, start);
            string rightStr = invariant_TextBox.Text.Substring(end);
            invariant_TextBox.Text = leftStr + symbol + rightStr;
            invariant_TextBox.SelectionStart = invariant_TextBox.SelectionEnd = start + symbol.Length;
        }

        #endregion

        #region 资源引用

        private ListBox ctl_ListBox, invariant_ListBox;
        private TextBox ctl_TextBox, invariant_TextBox;
        private ComboBox process_CTL_ComboBox, process_Inv_ComboBox;

        // 获取控件引用
        private void get_control_reference()
        {
            ctl_ListBox = ControlExtensions.FindControl<ListBox>(this, nameof(ctl_ListBox));
            invariant_ListBox = ControlExtensions.FindControl<ListBox>(this, nameof(invariant_ListBox));
            ctl_TextBox = ControlExtensions.FindControl<TextBox>(this, nameof(ctl_TextBox));
            invariant_TextBox = ControlExtensions.FindControl<TextBox>(this, nameof(invariant_TextBox));
            process_CTL_ComboBox = ControlExtensions.FindControl<ComboBox>(this, nameof(process_CTL_ComboBox));
            process_Inv_ComboBox = ControlExtensions.FindControl<ComboBox>(this, nameof(process_Inv_ComboBox));
        }

        public SafetyProperty_EW_VM VM { get => ((SafetyProperty_EW_VM)DataContext); }

        #endregion

        #region 事件

        // CTL页的属性导航器的进程模板选中项变化的处理
        private void process_CTL_ComboBox_Changed(object sender, SelectionChangedEventArgs e)
        {
            // 清空属性列表
            VM.CTLProperties.Clear();
            // 获取选中的Process
            if (process_CTL_ComboBox.SelectedItem == null)
            {
                return;
            }
            Process process = (Process)process_CTL_ComboBox.SelectedItem;
            // 构造这个属性列表
            foreach (Attribute attribute in process.Attributes)
            {
                Nav nav;
                if (attribute.Type is UserType) // 引用类型
                {
                    nav = ReferenceNav.build(attribute, null);
                }
                else // 值类型
                {
                    nav = new ValueNav(attribute, null);
                }
                VM.CTLProperties.Add(nav);
            }
        }

        // 不变性页的属性导航器的进程模板选中项变化的处理
        private void process_Inv_ComboBox_Changed(object sender, SelectionChangedEventArgs e)
        {
            // 清空属性列表
            VM.InvProperties.Clear();
            // 获取选中的Process
            if (process_Inv_ComboBox.SelectedItem == null)
            {
                return;
            }
            Process process = (Process)process_Inv_ComboBox.SelectedItem;
            // 构造这个属性列表
            foreach (Attribute attribute in process.Attributes)
            {
                Nav nav;
                if (attribute.Type is UserType) // 引用类型
                {
                    nav = ReferenceNav.build(attribute, null);
                }
                else // 值类型
                {
                    nav = new ValueNav(attribute, null);
                }
                VM.InvProperties.Add(nav);
            }
        }

        #endregion

        #region 初始化

        // 初始化.cs文件中的事件处理方法,一些无法在xaml中绑定的部分在这里绑定
        private void init_event()
        {
            // 绑定进程模板ComboBox选中项变化的处理方法
            process_CTL_ComboBox.SelectionChanged += process_CTL_ComboBox_Changed;
            process_Inv_ComboBox.SelectionChanged += process_Inv_ComboBox_Changed;
        }

        #endregion
    }
}
