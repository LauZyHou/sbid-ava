﻿using Avalonia;
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
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #region 按钮命令

        public void Add_CTL()
        {
            TextBox ctl_TextBox = ControlExtensions.FindControl<TextBox>(this, "ctl_TextBox");
            if (ctl_TextBox.Text == null || ctl_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要给出要添加的CTL公式！";
                return;
            }

            Formula ctl = new Formula(ctl_TextBox.Text);
            ((SafetyProperty_EW_VM)DataContext).SafetyProperty.CTLs.Add(ctl);
            ResourceManager.mainWindowVM.Tips = "添加了CTL公式：" + ctl.Content;
        }

        public void Update_CTL()
        {
            ListBox ctl_ListBox = ControlExtensions.FindControl<ListBox>(this, "ctl_ListBox");
            if (ctl_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要修改的CTL公式！";
                return;
            }

            TextBox ctl_TextBox = ControlExtensions.FindControl<TextBox>(this, "ctl_TextBox");
            if (ctl_TextBox.Text == null || ctl_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要给出修改后的CTL公式！";
                return;
            }

            Formula ctl = (Formula)ctl_ListBox.SelectedItem;
            ctl.Content = ctl_TextBox.Text;
            ResourceManager.mainWindowVM.Tips = "修改了CTL公式：" + ctl.Content;
        }

        public void Delete_CTL()
        {
            ListBox ctl_ListBox = ControlExtensions.FindControl<ListBox>(this, "ctl_ListBox");
            if (ctl_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要删除的CTL公式！";
                return;
            }

            Formula ctl = (Formula)ctl_ListBox.SelectedItem;
            ((SafetyProperty_EW_VM)DataContext).SafetyProperty.CTLs.Remove(ctl);
            ResourceManager.mainWindowVM.Tips = "删除了CTL公式：" + ctl.Content;
        }

        public void Add_Invariant()
        {
            TextBox invariant_TextBox = ControlExtensions.FindControl<TextBox>(this, "invariant_TextBox");
            if (invariant_TextBox.Text == null || invariant_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要给出要添加的不变性条件！";
                return;
            }

            Formula invariant = new Formula(invariant_TextBox.Text);
            ((SafetyProperty_EW_VM)DataContext).SafetyProperty.Invariants.Add(invariant);
            ResourceManager.mainWindowVM.Tips = "添加了不变性条件：" + invariant.Content;
        }

        public void Update_Invariant()
        {
            ListBox invariant_ListBox = ControlExtensions.FindControl<ListBox>(this, "invariant_ListBox");
            if (invariant_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要修改的不变性条件！";
                return;
            }

            TextBox invariant_TextBox = ControlExtensions.FindControl<TextBox>(this, "invariant_TextBox");
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
            ListBox invariant_ListBox = ControlExtensions.FindControl<ListBox>(this, "invariant_ListBox");
            if (invariant_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要删除的不变性条件！";
                return;
            }

            Formula invariant = (Formula)invariant_ListBox.SelectedItem;
            ((SafetyProperty_EW_VM)DataContext).SafetyProperty.Invariants.Remove(invariant);
            ResourceManager.mainWindowVM.Tips = "删除了不变性条件：" + invariant.Content;
        }

        #endregion
    }
}