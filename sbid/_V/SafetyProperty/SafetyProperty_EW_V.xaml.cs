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
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #region 按钮命令

        public void Add_CTL()
        {
            if (process_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定一个进程模板！";
                return;
            }
            Process process = (Process)process_ComboBox.SelectedItem;

            //if (state_ComboBox.SelectedItem == null)
            //{
            //    ResourceManager.mainWindowVM.Tips = "需要选定一个状态！";
            //    return;
            //}
            //State state = (State)state_ComboBox.SelectedItem;

            if (ctl_TextBox.Text == null || ctl_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要给出要添加的CTL公式！";
                return;
            }

            Formula formula = new Formula(ctl_TextBox.Text);
            CTL ctl = new CTL(process, formula);
            VM.SafetyProperty.CTLs.Add(ctl);
            ResourceManager.mainWindowVM.Tips = "添加了CTL公式：" + ctl;
        }

        public void Update_CTL()
        {
            if (ctl_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要修改的CTL公式！";
                return;
            }

            if (process_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定一个进程模板！";
                return;
            }
            Process process = (Process)process_ComboBox.SelectedItem;

            //if (state_ComboBox.SelectedItem == null)
            //{
            //    ResourceManager.mainWindowVM.Tips = "需要选定一个状态！";
            //    return;
            //}
            //State state = (State)state_ComboBox.SelectedItem;

            if (ctl_TextBox.Text == null || ctl_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要给出修改后的CTL公式！";
                return;
            }

            CTL ctl = (CTL)ctl_ListBox.SelectedItem;
            ctl.Process = process;
            // ctl.State = state;
            ctl.Formula.Content = ctl_TextBox.Text;
            ResourceManager.mainWindowVM.Tips = "修改了CTL公式：" + ctl;
        }

        public void Delete_CTL()
        {
            if (ctl_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要删除的CTL公式！";
                return;
            }

            CTL ctl = (CTL)ctl_ListBox.SelectedItem;
            VM.SafetyProperty.CTLs.Remove(ctl);
            ResourceManager.mainWindowVM.Tips = "删除了CTL公式：" + ctl;
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

        #region 资源引用

        private ListBox ctl_ListBox, invariant_ListBox;
        private TextBox ctl_TextBox, invariant_TextBox;
        private ComboBox process_ComboBox, state_ComboBox;

        // 获取控件引用
        private void get_control_reference()
        {
            ctl_ListBox = ControlExtensions.FindControl<ListBox>(this, nameof(ctl_ListBox));
            invariant_ListBox = ControlExtensions.FindControl<ListBox>(this, nameof(invariant_ListBox));
            process_ComboBox = ControlExtensions.FindControl<ComboBox>(this, nameof(process_ComboBox));
            // state_ComboBox = ControlExtensions.FindControl<ComboBox>(this, nameof(state_ComboBox));
            ctl_TextBox = ControlExtensions.FindControl<TextBox>(this, nameof(ctl_TextBox));
            invariant_TextBox = ControlExtensions.FindControl<TextBox>(this, nameof(invariant_TextBox));
        }

        public SafetyProperty_EW_VM VM { get => ((SafetyProperty_EW_VM)DataContext); }

        #endregion
    }
}
