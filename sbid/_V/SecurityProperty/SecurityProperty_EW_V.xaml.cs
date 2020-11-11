using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using sbid._M;
using sbid._VM;
using System.Collections.ObjectModel;

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
            Process process = (Process)process_Con_ListBox.SelectedItem;

            ListBox attribute_Con_ListBox = ControlExtensions.FindControl<ListBox>(this, "attribute_Con_ListBox");
            if (attribute_Con_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定Attribute！";
                return;
            }
            Attribute attribute = (Attribute)attribute_Con_ListBox.SelectedItem;

            ObservableCollection<Confidential> confidentials = ((SecurityProperty_EW_VM)DataContext).SecurityProperty.Confidentials;
            // 判重
            foreach (Confidential conf in confidentials)
            {
                if (conf.Process == process && conf.Attribute == attribute)
                {
                    ResourceManager.mainWindowVM.Tips = "无效的操作。该条机密性性质已存在";
                    return;
                }
            }

            Confidential confidential = new Confidential(process, attribute);
            confidentials.Add(confidential);
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
            Process process = (Process)process_Con_ListBox.SelectedItem;

            ListBox attribute_Con_ListBox = ControlExtensions.FindControl<ListBox>(this, "attribute_Con_ListBox");
            if (attribute_Con_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定Attribute！";
                return;
            }
            Attribute attribute = (Attribute)attribute_Con_ListBox.SelectedItem;

            Confidential confidential = (Confidential)confidential_ListBox.SelectedItem;
            ObservableCollection<Confidential> confidentials = ((SecurityProperty_EW_VM)DataContext).SecurityProperty.Confidentials;
            // 判重
            foreach (Confidential conf in confidentials)
            {
                if (conf.Process == process && conf.Attribute == attribute)
                {
                    ResourceManager.mainWindowVM.Tips = "无效的操作。该条机密性性质已存在";
                    return;
                }
            }

            confidential.Process = process;
            confidential.Attribute = attribute;
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
            Process processA = (Process)processA_ComboBox.SelectedItem;

            ComboBox stateA_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "stateA_ComboBox");
            if (stateA_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定StateA！";
                return;
            }
            State stateA = (State)stateA_ComboBox.SelectedItem;

            ComboBox attributeA_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "attributeA_ComboBox");
            if (attributeA_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定AttributeA！";
                return;
            }
            Attribute attributeA = (Attribute)attributeA_ComboBox.SelectedItem;

            ComboBox attributeA_Attr_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "attributeA_Attr_ComboBox");
            if (attributeA_Attr_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定AttributeA下的二级属性！";
                return;
            }
            Attribute attributeA_Attr = (Attribute)attributeA_Attr_ComboBox.SelectedItem;

            ComboBox processB_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "processB_ComboBox");
            if (processB_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定ProcessB！";
                return;
            }
            Process processB = (Process)processB_ComboBox.SelectedItem;

            ComboBox stateB_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "stateB_ComboBox");
            if (stateB_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定StateB！";
                return;
            }
            State stateB = (State)stateB_ComboBox.SelectedItem;

            ComboBox attributeB_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "attributeB_ComboBox");
            if (attributeB_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定AttributeB！";
                return;
            }
            Attribute attributeB = (Attribute)attributeB_ComboBox.SelectedItem;

            ComboBox attributeB_Attr_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "attributeB_Attr_ComboBox");
            if (attributeB_Attr_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定AttributeB下的二级属性！";
                return;
            }
            Attribute attributeB_Attr = (Attribute)attributeB_Attr_ComboBox.SelectedItem;

            ObservableCollection<Authenticity> authenticities = ((SecurityProperty_EW_VM)DataContext).SecurityProperty.Authenticities;
            // 判重
            foreach (Authenticity auth in authenticities)
            {
                if (
                    auth.ProcessA == processA && 
                    auth.StateA == stateA && 
                    auth.AttributeA == attributeA && 
                    auth.AttributeA_Attr == attributeA_Attr &&
                    auth.ProcessB == processB && 
                    auth.StateB == stateB && 
                    auth.AttributeB == attributeB && 
                    auth.AttributeB_Attr == attributeB_Attr
                    )
                {
                    ResourceManager.mainWindowVM.Tips = "无效的操作。该条认证性性质已存在";
                    return;
                }
            }

            Authenticity authenticity = new Authenticity(
                processA,
                stateA,
                attributeA,
                attributeA_Attr,
                processB,
                stateB,
                attributeB,
                attributeB_Attr
            );
            authenticities.Add(authenticity);
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
            Authenticity authenticity = (Authenticity)authenticity_ListBox.SelectedItem;

            ComboBox processA_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "processA_ComboBox");
            if (processA_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定ProcessA！";
                return;
            }
            Process processA = (Process)processA_ComboBox.SelectedItem;

            ComboBox stateA_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "stateA_ComboBox");
            if (stateA_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定StateA！";
                return;
            }
            State stateA = (State)stateA_ComboBox.SelectedItem;

            ComboBox attributeA_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "attributeA_ComboBox");
            if (attributeA_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定AttributeA！";
                return;
            }
            Attribute attributeA = (Attribute)attributeA_ComboBox.SelectedItem;

            ComboBox attributeA_Attr_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "attributeA_Attr_ComboBox");
            if (attributeA_Attr_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定AttributeA下的二级属性！";
                return;
            }
            Attribute attributeA_Attr = (Attribute)attributeA_Attr_ComboBox.SelectedItem;

            ComboBox processB_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "processB_ComboBox");
            if (processB_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定ProcessB！";
                return;
            }
            Process processB = (Process)processB_ComboBox.SelectedItem;

            ComboBox stateB_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "stateB_ComboBox");
            if (stateB_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定StateB！";
                return;
            }
            State stateB = (State)stateB_ComboBox.SelectedItem;

            ComboBox attributeB_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "attributeB_ComboBox");
            if (attributeB_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定AttributeB！";
                return;
            }
            Attribute attributeB = (Attribute)attributeB_ComboBox.SelectedItem;

            ComboBox attributeB_Attr_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "attributeB_Attr_ComboBox");
            if (attributeB_Attr_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定AttributeB下的二级属性！";
                return;
            }
            Attribute attributeB_Attr = (Attribute)attributeB_Attr_ComboBox.SelectedItem;

            ObservableCollection<Authenticity> authenticities = ((SecurityProperty_EW_VM)DataContext).SecurityProperty.Authenticities;
            // 判重
            foreach (Authenticity auth in authenticities)
            {
                if (
                    auth.ProcessA == processA &&
                    auth.StateA == stateA &&
                    auth.AttributeA == attributeA &&
                    auth.AttributeA_Attr == attributeA_Attr &&
                    auth.ProcessB == processB &&
                    auth.StateB == stateB &&
                    auth.AttributeB == attributeB &&
                    auth.AttributeB_Attr == attributeB_Attr
                    )
                {
                    ResourceManager.mainWindowVM.Tips = "无效的操作。该条认证性性质已存在";
                    return;
                }
            }

            authenticity.ProcessA = processA;
            authenticity.StateA = stateA;
            authenticity.AttributeA = attributeA;
            authenticity.AttributeA_Attr = attributeA_Attr;
            authenticity.ProcessB = processB;
            authenticity.StateB = stateB;
            authenticity.AttributeB = attributeB;
            authenticity.AttributeB_Attr = attributeB_Attr;
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
            Process processA = (Process)it_ProcessA_ComboBox.SelectedItem;

            ComboBox it_StateA_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "it_StateA_ComboBox");
            if (it_StateA_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定StateA！";
                return;
            }
            State stateA = (State)it_StateA_ComboBox.SelectedItem;

            ComboBox it_AttributeA_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "it_AttributeA_ComboBox");
            if (it_AttributeA_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定AttributeA！";
                return;
            }
            Attribute attributeA = (Attribute)it_AttributeA_ComboBox.SelectedItem;

            ComboBox it_ProcessB_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "it_ProcessB_ComboBox");
            if (it_ProcessB_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定ProcessB！";
                return;
            }
            Process processB = (Process)it_ProcessB_ComboBox.SelectedItem;

            ComboBox it_StateB_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "it_StateB_ComboBox");
            if (it_StateB_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定StateB！";
                return;
            }
            State stateB = (State)it_StateB_ComboBox.SelectedItem;

            ComboBox it_AttributeB_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "it_AttributeB_ComboBox");
            if (it_AttributeB_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定AttributeB！";
                return;
            }
            Attribute attributeB = (Attribute)it_AttributeB_ComboBox.SelectedItem;

            ObservableCollection<Integrity> integrities = ((SecurityProperty_EW_VM)DataContext).SecurityProperty.Integrities;
            // 判重
            foreach (Integrity inte in integrities)
            {
                if (
                    inte.ProcessA == processA &&
                    inte.StateA == stateA &&
                    inte.AttributeA == attributeA &&
                    inte.ProcessB == processB &&
                    inte.StateB == stateB &&
                    inte.AttributeB == attributeB
                    )
                {
                    ResourceManager.mainWindowVM.Tips = "无效的操作。该条完整性性质已存在";
                    return;
                }
            }

            Integrity integrity = new Integrity(
                processA,
                stateA,
                attributeA,
                processB,
                stateB,
                attributeB
            );
            integrities.Add(integrity);
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
            Integrity integrity = (Integrity)integrity_ListBox.SelectedItem;

            ComboBox it_ProcessA_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "it_ProcessA_ComboBox");
            if (it_ProcessA_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定ProcessA！";
                return;
            }
            Process processA = (Process)it_ProcessA_ComboBox.SelectedItem;

            ComboBox it_StateA_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "it_StateA_ComboBox");
            if (it_StateA_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定StateA！";
                return;
            }
            State stateA = (State)it_StateA_ComboBox.SelectedItem;

            ComboBox it_AttributeA_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "it_AttributeA_ComboBox");
            if (it_AttributeA_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定AttributeA！";
                return;
            }
            Attribute attributeA = (Attribute)it_AttributeA_ComboBox.SelectedItem;

            ComboBox it_ProcessB_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "it_ProcessB_ComboBox");
            if (it_ProcessB_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定ProcessB！";
                return;
            }
            Process processB = (Process)it_ProcessB_ComboBox.SelectedItem;

            ComboBox it_StateB_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "it_StateB_ComboBox");
            if (it_StateB_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定StateB！";
                return;
            }
            State stateB = (State)it_StateB_ComboBox.SelectedItem;

            ComboBox it_AttributeB_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "it_AttributeB_ComboBox");
            if (it_AttributeB_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定AttributeB！";
                return;
            }
            Attribute attributeB = (Attribute)it_AttributeB_ComboBox.SelectedItem;

            ObservableCollection<Integrity> integrities = ((SecurityProperty_EW_VM)DataContext).SecurityProperty.Integrities;
            // 判重
            foreach (Integrity inte in integrities)
            {
                if (
                    inte.ProcessA == processA &&
                    inte.StateA == stateA &&
                    inte.AttributeA == attributeA &&
                    inte.ProcessB == processB &&
                    inte.StateB == stateB &&
                    inte.AttributeB == attributeB
                    )
                {
                    ResourceManager.mainWindowVM.Tips = "无效的操作。该条完整性性质已存在";
                    return;
                }
            }

            integrity.ProcessA = processA;
            integrity.StateA = stateA;
            integrity.AttributeA = attributeA;
            integrity.ProcessB = processB;
            integrity.StateB = stateB;
            integrity.AttributeB = attributeB;
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

            ObservableCollection<Availability> availabilities = VM.SecurityProperty.Availabilities;
            // 判重
            foreach (Availability avai in availabilities)
            {
                if (avai.Process == process && avai.State == state)
                {
                    ResourceManager.mainWindowVM.Tips = "无效的操作。该条可用性性质已存在";
                    return;
                }
            }

            Availability availability = new Availability(process, state);
            availabilities.Add(availability);
            ResourceManager.mainWindowVM.Tips = "添加了Availability：" + availability;
        }

        public void Update_Availability()
        {
            if (availability_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定可用性！";
                return;
            }
            Availability availability = (Availability)availability_ListBox.SelectedItem;

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

            ObservableCollection<Availability> availabilities = VM.SecurityProperty.Availabilities;
            // 判重
            foreach (Availability avai in availabilities)
            {
                if (avai.Process == process && avai.State == state)
                {
                    ResourceManager.mainWindowVM.Tips = "无效的操作。该条可用性性质已存在";
                    return;
                }
            }

            availability.Process = process;
            availability.State = state;
            ResourceManager.mainWindowVM.Tips = "修改了Availability：" + availability;
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
