using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using sbid._M;
using sbid._VM;

namespace sbid._V
{
    public class AtomProposition_EW_V : Window
    {
        public AtomProposition_EW_V()
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

        #region 按钮命令

        // 属性导航器【接入】
        public void Append_PropNav()
        {
            if (process_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定进程模板！";
                return;
            }
            Process process = (Process)process_ComboBox.SelectedItem;

            if (propNav_TreeView.SelectedItem == null)
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
            navStr = process.RefName.Content + "." + navStr;

            Append_Symbol(navStr);
        }


        // 向ap_TextBox中插入字符串symbol
        // 在光标所在位置插入symbol，如果选中了一段，将这段替换为插入的symbol
        private void Append_Symbol(string symbol)
        {
            if (ap_TextBox.Text == null)
            {
                ap_TextBox.Text = "";
            }
            int start = ap_TextBox.SelectionStart;
            int end = ap_TextBox.SelectionEnd;
            // 可能会鼠标从右往左选择，所以要判断交换两者的值，保证start在左边
            if (end < start)
            {
                int tmp = start;
                start = end;
                end = tmp;
            }
            string leftStr = ap_TextBox.Text.Substring(0, start);
            string rightStr = ap_TextBox.Text.Substring(end);
            ap_TextBox.Text = leftStr + symbol + rightStr;
            ap_TextBox.SelectionStart = ap_TextBox.SelectionEnd = start + symbol.Length;
        }

        #endregion

        #region 事件

        // 属性导航器的进程模板选中项变化的处理
        private void process_ComboBox_Changed(object sender, SelectionChangedEventArgs e)
        {
            // 清空属性列表
            VM.Properties.Clear();
            // 获取选中的Process
            if (process_ComboBox.SelectedItem == null)
            {
                return;
            }
            Process process = (Process)process_ComboBox.SelectedItem;
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
                VM.Properties.Add(nav);
            }
        }

        #endregion

        #region 初始化

        // 初始化.cs文件中的事件处理方法,一些无法在xaml中绑定的部分在这里绑定
        private void init_event()
        {
            // 绑定进程模板ComboBox选中项变化的处理方法
            process_ComboBox.SelectionChanged += process_ComboBox_Changed;
        }

        #endregion

        #region 资源引用

        private ComboBox process_ComboBox;
        private TreeView propNav_TreeView;
        private TextBox ap_TextBox;

        // 获取控件引用
        private void get_control_reference()
        {
            process_ComboBox = ControlExtensions.FindControl<ComboBox>(this, nameof(process_ComboBox));
            propNav_TreeView = ControlExtensions.FindControl<TreeView>(this, nameof(propNav_TreeView));
            ap_TextBox = ControlExtensions.FindControl<TextBox>(this, nameof(ap_TextBox));
        }

        public AtomProposition_EW_VM VM { get => (AtomProposition_EW_VM)DataContext; }

        #endregion
    }
}
