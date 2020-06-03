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
            // 获取控件引用
            this.get_control_reference();
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
            ResourceManager.mainWindowVM.Tips = "关联到Process：" + process.RefName;
        }

        public void Add_Knowledge()
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

            Knowledge knowledge = new Knowledge((Process)process_ListBox.SelectedItem, (Attribute)attribute_ListBox.SelectedItem);
            ((InitialKnowledge_EW_VM)DataContext).InitialKnowledge.Knowledges.Add(knowledge);
            ResourceManager.mainWindowVM.Tips = "添加了Knowledge：" + knowledge;
        }

        public void Update_Knowledge()
        {
            ListBox knowledge_ListBox = ControlExtensions.FindControl<ListBox>(this, "knowledge_ListBox");
            if (knowledge_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要修改的Knowledge！";
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

            Knowledge knowledge = (Knowledge)knowledge_ListBox.SelectedItem;
            knowledge.Process = (Process)process_ListBox.SelectedItem;
            knowledge.Attribute = (Attribute)attribute_ListBox.SelectedItem;
            ResourceManager.mainWindowVM.Tips = "修改了Knowledge：" + knowledge;
        }

        public void Delete_Knowledge()
        {
            ListBox knowledge_ListBox = ControlExtensions.FindControl<ListBox>(this, "knowledge_ListBox");
            if (knowledge_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要删除的Knowledge！";
                return;
            }

            Knowledge knowledge = (Knowledge)knowledge_ListBox.SelectedItem;
            ((InitialKnowledge_EW_VM)DataContext).InitialKnowledge.Knowledges.Remove(knowledge);
            ResourceManager.mainWindowVM.Tips = "删除了Knowledge：" + knowledge;
        }

        public void Add_KeyPair()
        {
            if (pubProcess_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定PubProcess！";
                return;
            }
            if (pubKey_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定PubKey！";
                return;
            }
            if (secProcess_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定SecProcess！";
                return;
            }
            if (secKey_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定SecKey！";
                return;
            }

            KeyPair keyPair = new KeyPair(
                (Process)pubProcess_ComboBox.SelectedItem,
                (Attribute)pubKey_ComboBox.SelectedItem,
                (Process)secProcess_ComboBox.SelectedItem,
                (Attribute)secKey_ComboBox.SelectedItem
            );
            ((InitialKnowledge_EW_VM)DataContext).InitialKnowledge.KeyPairs.Add(keyPair);
            ResourceManager.mainWindowVM.Tips = "添加了KeyPair：" + keyPair;
        }


        public void Update_KeyPair()
        {
            if (pubProcess_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定PubProcess！";
                return;
            }
            if (pubKey_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定PubKey！";
                return;
            }
            if (secProcess_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定SecProcess！";
                return;
            }
            if (secKey_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定SecKey！";
                return;
            }
            if(keyPair_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要修改的KeyPair！";
                return;
            }
        }

        public void Delete_KeyPair()
        {

        }

        #endregion


        #region 控件引用

        private ComboBox pubProcess_ComboBox, pubKey_ComboBox, secProcess_ComboBox, secKey_ComboBox;

        private ListBox keyPair_ListBox;

        // 获取控件引用
        private void get_control_reference()
        {
            pubProcess_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "pubProcess_ComboBox");
            pubKey_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "pubKey_ComboBox");
            secProcess_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "secProcess_ComboBox");
            secKey_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "secKey_ComboBox");
            keyPair_ListBox = ControlExtensions.FindControl<ListBox>(this, "keyPair_ListBox");
        }

        #endregion
    }
}
