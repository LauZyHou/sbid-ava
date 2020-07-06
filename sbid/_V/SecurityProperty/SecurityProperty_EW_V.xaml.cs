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
            this.get_control_reference();
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
            ComboBox attributeA_Attr_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "attributeA_Attr_ComboBox");
            if (attributeA_Attr_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定AttributeA下的二级属性！";
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
            ComboBox attributeB_Attr_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "attributeB_Attr_ComboBox");
            if (attributeB_Attr_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定AttributeB下的二级属性！";
                return;
            }

            Authenticity authenticity = new Authenticity(
                (Process)processA_ComboBox.SelectedItem,
                (State)stateA_ComboBox.SelectedItem,
                (Attribute)attributeA_ComboBox.SelectedItem,
                (Attribute)attributeA_Attr_ComboBox.SelectedItem,
                (Process)processB_ComboBox.SelectedItem,
                (State)stateB_ComboBox.SelectedItem,
                (Attribute)attributeB_ComboBox.SelectedItem,
                (Attribute)attributeB_Attr_ComboBox.SelectedItem
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

        public void Add_Integrity()
        {
            ComboBox it_ProcessA_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "it_ProcessA_ComboBox");
            if (it_ProcessA_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定ProcessA！";
                return;
            }
            ComboBox it_StateA_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "it_StateA_ComboBox");
            if (it_StateA_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定StateA！";
                return;
            }
            ComboBox it_AttributeA_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "it_AttributeA_ComboBox");
            if (it_AttributeA_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定AttributeA！";
                return;
            }
            ComboBox it_ProcessB_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "it_ProcessB_ComboBox");
            if (it_ProcessB_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定ProcessB！";
                return;
            }
            ComboBox it_StateB_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "it_StateB_ComboBox");
            if (it_StateB_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定StateB！";
                return;
            }
            ComboBox it_AttributeB_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "it_AttributeB_ComboBox");
            if (it_AttributeB_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定AttributeB！";
                return;
            }

            Integrity integrity = new Integrity(
                (Process)it_ProcessA_ComboBox.SelectedItem,
                (State)it_StateA_ComboBox.SelectedItem,
                (Attribute)it_AttributeA_ComboBox.SelectedItem,
                (Process)it_ProcessB_ComboBox.SelectedItem,
                (State)it_StateB_ComboBox.SelectedItem,
                (Attribute)it_AttributeB_ComboBox.SelectedItem
            );
            ((SecurityProperty_EW_VM)DataContext).SecurityProperty.Integrities.Add(integrity);
            ResourceManager.mainWindowVM.Tips = "添加了Integrity：" + integrity;
        }

        public void Update_Integrity()
        {
            ListBox integrity_ListBox = ControlExtensions.FindControl<ListBox>(this, "integrity_ListBox");
            if (integrity_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要修改的Integrity！";
                return;
            }

            ComboBox it_ProcessA_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "it_ProcessA_ComboBox");
            if (it_ProcessA_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定ProcessA！";
                return;
            }
            ComboBox it_StateA_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "it_StateA_ComboBox");
            if (it_StateA_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定StateA！";
                return;
            }
            ComboBox it_AttributeA_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "it_AttributeA_ComboBox");
            if (it_AttributeA_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定AttributeA！";
                return;
            }
            ComboBox it_ProcessB_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "it_ProcessB_ComboBox");
            if (it_ProcessB_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定ProcessB！";
                return;
            }
            ComboBox it_StateB_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "it_StateB_ComboBox");
            if (it_StateB_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定StateB！";
                return;
            }
            ComboBox it_AttributeB_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "it_AttributeB_ComboBox");
            if (it_AttributeB_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定AttributeB！";
                return;
            }

            Integrity integrity = (Integrity)integrity_ListBox.SelectedItem;
            integrity.ProcessA = (Process)it_ProcessA_ComboBox.SelectedItem;
            integrity.StateA = (State)it_StateA_ComboBox.SelectedItem;
            integrity.AttributeA = (Attribute)it_AttributeA_ComboBox.SelectedItem;
            integrity.ProcessB = (Process)it_ProcessB_ComboBox.SelectedItem;
            integrity.StateB = (State)it_StateB_ComboBox.SelectedItem;
            integrity.AttributeB = (Attribute)it_AttributeB_ComboBox.SelectedItem;
            ResourceManager.mainWindowVM.Tips = "修改了Integrity：" + integrity;
        }

        public void Delete_Integrity()
        {
            ListBox integrity_ListBox = ControlExtensions.FindControl<ListBox>(this, "integrity_ListBox");
            if (integrity_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要删除的Integrity！";
                return;
            }

            Integrity integrity = (Integrity)integrity_ListBox.SelectedItem;
            ((SecurityProperty_EW_VM)DataContext).SecurityProperty.Integrities.Remove(integrity);
            ResourceManager.mainWindowVM.Tips = "删除了Integrity：" + integrity;
        }

        public void Add_Availability()
        {
            if (process_Ava_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定进程模板！";
                return;
            }
            Process process = (Process)process_Ava_ListBox.SelectedItem;

            if (state_Ava_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定状态！";
                return;
            }
            State state = (State)state_Ava_ListBox.SelectedItem;

            Availability availability = new Availability(process, state);
            VM.SecurityProperty.Availabilities.Add(availability);
            ResourceManager.mainWindowVM.Tips = "添加了可用性：" + availability;
        }

        public void Update_Availability()
        {
            if (process_Ava_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定进程模板！";
                return;
            }
            Process process = (Process)process_Ava_ListBox.SelectedItem;

            if (state_Ava_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定状态！";
                return;
            }
            State state = (State)state_Ava_ListBox.SelectedItem;

            if (availability_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定可用性！";
                return;
            }
            Availability availability = (Availability)availability_ListBox.SelectedItem;

            availability.Process = process;
            availability.State = state;
            ResourceManager.mainWindowVM.Tips = "修改了可用性：" + availability;
        }

        public void Delete_Availability()
        {
            if (availability_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定可用性！";
                return;
            }
            Availability availability = (Availability)availability_ListBox.SelectedItem;

            VM.SecurityProperty.Availabilities.Remove(availability);
            ResourceManager.mainWindowVM.Tips = "删除了可用性：" + availability;
        }

        #endregion

        #region 资源引用

        private ListBox process_Ava_ListBox, state_Ava_ListBox, availability_ListBox;

        // 获取控件引用
        private void get_control_reference()
        {
            process_Ava_ListBox = ControlExtensions.FindControl<ListBox>(this, nameof(process_Ava_ListBox));
            state_Ava_ListBox = ControlExtensions.FindControl<ListBox>(this, nameof(state_Ava_ListBox));
            availability_ListBox = ControlExtensions.FindControl<ListBox>(this, nameof(availability_ListBox));
        }

        public SecurityProperty_EW_VM VM { get => ((SecurityProperty_EW_VM)DataContext); }

        #endregion
    }
}
