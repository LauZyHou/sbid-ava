using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using sbid._M;
using sbid._VM;

namespace sbid._V
{
    public class SecurityProperty_EW_V : Window
    {
        public SecurityProperty_EW_V()
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

        public void Add_Confidential()
        {
            ListBox process_Con_ListBox = ControlExtensions.FindControl<ListBox>(this, "process_Con_ListBox");
            if (process_Con_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定Process！";
                return;
            }

            ListBox attribute_Con_ListBox = ControlExtensions.FindControl<ListBox>(this, "attribute_Con_ListBox");
            if (attribute_Con_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定Attribute！";
                return;
            }

            Confidential confidential = new Confidential((Process)process_Con_ListBox.SelectedItem, (Attribute)attribute_Con_ListBox.SelectedItem);
            ((SecurityProperty_EW_VM)DataContext).SecurityProperty.Confidentials.Add(confidential);
            ResourceManager.mainWindowVM.Tips = "添加了Confidential：" + confidential;
        }

        public void Update_Confidential()
        {
            ListBox confidential_ListBox = ControlExtensions.FindControl<ListBox>(this, "confidential_ListBox");
            if (confidential_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要修改的Confidential！";
                return;
            }

            ListBox process_Con_ListBox = ControlExtensions.FindControl<ListBox>(this, "process_Con_ListBox");
            if (process_Con_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定Process！";
                return;
            }

            ListBox attribute_Con_ListBox = ControlExtensions.FindControl<ListBox>(this, "attribute_Con_ListBox");
            if (attribute_Con_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定Attribute！";
                return;
            }

            Confidential confidential = (Confidential)confidential_ListBox.SelectedItem;
            confidential.Process = (Process)process_Con_ListBox.SelectedItem;
            confidential.Attribute = (Attribute)attribute_Con_ListBox.SelectedItem;
            ResourceManager.mainWindowVM.Tips = "修改了Confidential：" + confidential;
        }

        public void Delete_Confidential()
        {
            ListBox confidential_ListBox = ControlExtensions.FindControl<ListBox>(this, "confidential_ListBox");
            if (confidential_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要删除的Confidential！";
                return;
            }

            Confidential confidential = (Confidential)confidential_ListBox.SelectedItem;
            ((SecurityProperty_EW_VM)DataContext).SecurityProperty.Confidentials.Remove(confidential);
            ResourceManager.mainWindowVM.Tips = "删除了Confidential：" + confidential;
        }

        public void Add_Authenticity()
        {
            ComboBox processA_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "processA_ComboBox");
            if (processA_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定ProcessA！";
                return;
            }
            ComboBox stateA_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "stateA_ComboBox");
            if (stateA_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定StateA！";
                return;
            }
            ComboBox attributeA_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "attributeA_ComboBox");
            if (attributeA_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定AttributeA！";
                return;
            }
            ComboBox processB_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "processB_ComboBox");
            if (processB_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定ProcessB！";
                return;
            }
            ComboBox stateB_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "stateB_ComboBox");
            if (stateB_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定StateB！";
                return;
            }
            ComboBox attributeB_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "attributeB_ComboBox");
            if (attributeB_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定AttributeB！";
                return;
            }

            Authenticity authenticity = new Authenticity(
                (Process)processA_ComboBox.SelectedItem,
                (State)stateA_ComboBox.SelectedItem,
                (Attribute)attributeA_ComboBox.SelectedItem,
                (Process)processB_ComboBox.SelectedItem,
                (State)stateB_ComboBox.SelectedItem,
                (Attribute)attributeB_ComboBox.SelectedItem
            );
            ((SecurityProperty_EW_VM)DataContext).SecurityProperty.Authenticities.Add(authenticity);
            ResourceManager.mainWindowVM.Tips = "添加了Authenticity：" + authenticity;
        }

        public void Update_Authenticity()
        {
            ListBox authenticity_ListBox = ControlExtensions.FindControl<ListBox>(this, "authenticity_ListBox");
            if (authenticity_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要修改的Authenticity！";
                return;
            }

            ComboBox processA_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "processA_ComboBox");
            if (processA_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定ProcessA！";
                return;
            }
            ComboBox stateA_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "stateA_ComboBox");
            if (stateA_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定StateA！";
                return;
            }
            ComboBox attributeA_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "attributeA_ComboBox");
            if (attributeA_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定AttributeA！";
                return;
            }
            ComboBox processB_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "processB_ComboBox");
            if (processB_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定ProcessB！";
                return;
            }
            ComboBox stateB_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "stateB_ComboBox");
            if (stateB_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定StateB！";
                return;
            }
            ComboBox attributeB_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "attributeB_ComboBox");
            if (attributeB_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定AttributeB！";
                return;
            }

            Authenticity authenticity = (Authenticity)authenticity_ListBox.SelectedItem;
            authenticity.ProcessA = (Process)processA_ComboBox.SelectedItem;
            authenticity.StateA = (State)stateA_ComboBox.SelectedItem;
            authenticity.AttributeA = (Attribute)attributeA_ComboBox.SelectedItem;
            authenticity.ProcessB = (Process)processB_ComboBox.SelectedItem;
            authenticity.StateB = (State)stateB_ComboBox.SelectedItem;
            authenticity.AttributeB = (Attribute)attributeB_ComboBox.SelectedItem;
            ResourceManager.mainWindowVM.Tips = "修改了Authenticity：" + authenticity;
        }

        public void Delete_Authenticity()
        {
            ListBox authenticity_ListBox = ControlExtensions.FindControl<ListBox>(this, "authenticity_ListBox");
            if (authenticity_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要删除的Authenticity！";
                return;
            }

            Authenticity authenticity = (Authenticity)authenticity_ListBox.SelectedItem;
            ((SecurityProperty_EW_VM)DataContext).SecurityProperty.Authenticities.Remove(authenticity);
            ResourceManager.mainWindowVM.Tips = "删除了Authenticity：" + authenticity;
        }

        #endregion
    }
}
