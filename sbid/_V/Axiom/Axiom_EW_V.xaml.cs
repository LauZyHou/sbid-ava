using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using sbid._M;
using sbid._VM;
using System.Collections.ObjectModel;

namespace sbid._V
{
    public class Axiom_EW_V : Window
    {
        public Axiom_EW_V()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            // 初始化.cs文件中的数据绑定
            init_binding();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #region 按钮命令

        public void Add_ProcessMethod()
        {
            ListBox process_ListBox = ControlExtensions.FindControl<ListBox>(this, "process_ListBox");
            if (process_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定进程模板！";
                return;
            }
            Process process = (Process)process_ListBox.SelectedItem;

            ListBox method_ListBox = ControlExtensions.FindControl<ListBox>(this, "method_ListBox");
            if (method_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定方法！";
                return;
            }
            Method method = (Method)method_ListBox.SelectedItem;

            ProcessMethod processMethod = new ProcessMethod(process, method);
            ((Axiom_EW_VM)DataContext).Axiom.ProcessMethods.Add(processMethod);
            ResourceManager.mainWindowVM.Tips = "添加了ProcessMethod：" + processMethod;
        }

        public void Update_ProcessMethod()
        {
            ListBox processMethod_ListBox = ControlExtensions.FindControl<ListBox>(this, "processMethod_ListBox");
            if (processMethod_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要更新的ProcessMethod！";
                return;
            }
            ProcessMethod processMethod = (ProcessMethod)processMethod_ListBox.SelectedItem;

            ListBox process_ListBox = ControlExtensions.FindControl<ListBox>(this, "process_ListBox");
            if (process_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定进程模板！";
                return;
            }
            Process process = (Process)process_ListBox.SelectedItem;

            ListBox method_ListBox = ControlExtensions.FindControl<ListBox>(this, "method_ListBox");
            if (method_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定方法！";
                return;
            }
            Method method = (Method)method_ListBox.SelectedItem;

            processMethod.Process = process;
            processMethod.Method = method;
            ResourceManager.mainWindowVM.Tips = "更新了ProcessMethod：" + processMethod;
        }

        public void Delete_ProcessMethod()
        {
            ListBox processMethod_ListBox = ControlExtensions.FindControl<ListBox>(this, "processMethod_ListBox");
            if (processMethod_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要删除的ProcessMethod！";
                return;
            }
            ProcessMethod processMethod = (ProcessMethod)processMethod_ListBox.SelectedItem;
            ((Axiom_EW_VM)DataContext).Axiom.ProcessMethods.Remove(processMethod);
            ResourceManager.mainWindowVM.Tips = "删除了ProcessMethod：" + processMethod;
        }

        public void Add_Formula()
        {
            TextBox axiom_TextBox = ControlExtensions.FindControl<TextBox>(this, "axiom_TextBox");
            if (axiom_TextBox.Text == null || axiom_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要给出要添加的公理公式！";
                return;
            }

            Formula formula = new Formula(axiom_TextBox.Text);
            ((Axiom_EW_VM)DataContext).Axiom.Formulas.Add(formula);
            ResourceManager.mainWindowVM.Tips = "添加了公理公式：" + formula.Content;
        }

        public void Update_Formula()
        {
            ListBox axiom_ListBox = ControlExtensions.FindControl<ListBox>(this, "axiom_ListBox");
            if (axiom_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要更新的公理公式！";
                return;
            }

            TextBox axiom_TextBox = ControlExtensions.FindControl<TextBox>(this, "axiom_TextBox");
            if (axiom_TextBox.Text == null || axiom_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要给出修改后的公理公式！";
                return;
            }

            Formula formula = (Formula)axiom_ListBox.SelectedItem;
            formula.Content = axiom_TextBox.Text;
            ResourceManager.mainWindowVM.Tips = "更新了公理公式：" + formula.Content;
        }

        public void Delete_Formula()
        {
            ListBox axiom_ListBox = ControlExtensions.FindControl<ListBox>(this, "axiom_ListBox");
            if (axiom_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要删除的公理公式！";
                return;
            }

            Formula formula = (Formula)axiom_ListBox.SelectedItem;
            ((Axiom_EW_VM)DataContext).Axiom.Formulas.Remove(formula);
            ResourceManager.mainWindowVM.Tips = "删除了公理公式：" + formula.Content;
        }

        #endregion

        #region 初始化

        private void init_binding()
        {
            // 绑定Axiom的内置公理公式
            ListBox innerFormula_ListBox = ControlExtensions.FindControl<ListBox>(this, "innerFormula_ListBox");
            innerFormula_ListBox.Items = Axiom.InnerFormulas;
        }

        #endregion
    }
}
