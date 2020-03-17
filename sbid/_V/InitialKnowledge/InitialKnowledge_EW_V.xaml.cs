using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using sbid._M;
using sbid._VM;

namespace sbid._V
{
    public class InitialKnowledge_EW_V : Window
    {
        public InitialKnowledge_EW_V()
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

        public void Set_Process()
        {
            ComboBox process_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "process_ComboBox");
            if (process_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定Process！";
                return;
            }
            Process process = (Process)process_ComboBox.SelectedItem;

            ((InitialKnowledge_EW_VM)DataContext).InitialKnowledge.Process = process;
            ResourceManager.mainWindowVM.Tips = "关联到Process：" + process.Name;
        }

        public void Add_KnowledgePair()
        {
            ListBox process_ListBox = ControlExtensions.FindControl<ListBox>(this, "process_ListBox");
            if (process_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定Process！";
                return;
            }

            ListBox attribute_ListBox = ControlExtensions.FindControl<ListBox>(this, "attribute_ListBox");
            if (attribute_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定Attribute！";
                return;
            }

            KnowledgePair knowledgePair = new KnowledgePair((Process)process_ListBox.SelectedItem, (Attribute)attribute_ListBox.SelectedItem);
            ((InitialKnowledge_EW_VM)DataContext).InitialKnowledge.KnowledgePairs.Add(knowledgePair);
            ResourceManager.mainWindowVM.Tips = "添加了KnowledgePair：" + knowledgePair;
        }

        public void Update_KnowledgePair()
        {
            ListBox knowledgePair_ListBox = ControlExtensions.FindControl<ListBox>(this, "knowledgePair_ListBox");
            if (knowledgePair_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要修改的KnowledgePair！";
                return;
            }

            ListBox process_ListBox = ControlExtensions.FindControl<ListBox>(this, "process_ListBox");
            if (process_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定Process！";
                return;
            }

            ListBox attribute_ListBox = ControlExtensions.FindControl<ListBox>(this, "attribute_ListBox");
            if (attribute_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定Attribute！";
                return;
            }

            KnowledgePair knowledgePair = (KnowledgePair)knowledgePair_ListBox.SelectedItem;
            knowledgePair.Process = (Process)process_ListBox.SelectedItem;
            knowledgePair.Attribute = (Attribute)attribute_ListBox.SelectedItem;
            ResourceManager.mainWindowVM.Tips = "修改了KnowledgePair：" + knowledgePair;
        }

        public void Delete_KnowledgePair()
        {
            ListBox knowledgePair_ListBox = ControlExtensions.FindControl<ListBox>(this, "knowledgePair_ListBox");
            if (knowledgePair_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要删除的KnowledgePair！";
                return;
            }

            KnowledgePair knowledgePair = (KnowledgePair)knowledgePair_ListBox.SelectedItem;
            ((InitialKnowledge_EW_VM)DataContext).InitialKnowledge.KnowledgePairs.Remove(knowledgePair);
            ResourceManager.mainWindowVM.Tips = "删除了KnowledgePair：" + knowledgePair;
        }

        #endregion
    }
}
