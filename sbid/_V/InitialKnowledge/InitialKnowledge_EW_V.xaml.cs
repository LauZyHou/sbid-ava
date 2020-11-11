using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using sbid._M;
using sbid._VM;
using System.Collections.ObjectModel;

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
            Process process = (Process)process_ListBox.SelectedItem;

            ListBox attribute_ListBox = ControlExtensions.FindControl<ListBox>(this, "attribute_ListBox");
            if (attribute_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定Attribute！";
                return;
            }
            Attribute attribute = (Attribute)attribute_ListBox.SelectedItem;

            ObservableCollection<Knowledge> knowledges = ((InitialKnowledge_EW_VM)DataContext).InitialKnowledge.Knowledges;
            // 判重
            foreach (Knowledge know in knowledges)
            {
                if (know.Process == process && know.Attribute == attribute)
                {
                    ResourceManager.mainWindowVM.Tips = "无效的操作。该单知识已经存在";
                    return;
                }
            }

            Knowledge knowledge = new Knowledge(process, attribute);
            knowledges.Add(knowledge);
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
            Knowledge knowledge = (Knowledge)knowledge_ListBox.SelectedItem;

            ListBox process_ListBox = ControlExtensions.FindControl<ListBox>(this, "process_ListBox");
            if (process_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定Process！";
                return;
            }
            Process process = (Process)process_ListBox.SelectedItem;

            ListBox attribute_ListBox = ControlExtensions.FindControl<ListBox>(this, "attribute_ListBox");
            if (attribute_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定Attribute！";
                return;
            }
            Attribute attribute = (Attribute)attribute_ListBox.SelectedItem;

            ObservableCollection<Knowledge> knowledges = ((InitialKnowledge_EW_VM)DataContext).InitialKnowledge.Knowledges;
            // 判重
            foreach (Knowledge know in knowledges)
            {
                if (know.Process == process && know.Attribute == attribute)
                {
                    ResourceManager.mainWindowVM.Tips = "无效的操作。该单知识已经存在";
                    return;
                }
            }

            knowledge.Process = process;
            knowledge.Attribute = attribute;
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
            Process pubProcess = (Process)pubProcess_ComboBox.SelectedItem;

            if (pubKey_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定PubKey！";
                return;
            }
            Attribute pubKey = (Attribute)pubKey_ComboBox.SelectedItem;

            if (secProcess_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定SecProcess！";
                return;
            }
            Process secProcess = (Process)secProcess_ComboBox.SelectedItem;

            if (secKey_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定SecKey！";
                return;
            }
            Attribute secKey = (Attribute)secKey_ComboBox.SelectedItem;

            ObservableCollection<KeyPair> keyPairs = VM.InitialKnowledge.KeyPairs;
            // 判重
            foreach (KeyPair kp in keyPairs)
            {
                if (
                    kp.PubProcess == pubProcess &&
                    kp.PubKey == pubKey && 
                    kp.SecProcess == secProcess &&
                    kp.SecKey == secKey
                    )
                {
                    ResourceManager.mainWindowVM.Tips = "无效的操作。该公私钥对已经存在";
                    return;
                }
            }

            KeyPair keyPair = new KeyPair(
                pubProcess,
                pubKey,
                secProcess,
                secKey
            );
            keyPairs.Add(keyPair);
            ResourceManager.mainWindowVM.Tips = "添加了KeyPair：" + keyPair;
        }


        public void Update_KeyPair()
        {
            if (keyPair_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要修改的KeyPair！";
                return;
            }
            KeyPair keyPair = (KeyPair)keyPair_ListBox.SelectedItem;

            if (pubProcess_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定PubProcess！";
                return;
            }
            Process pubProcess = (Process)pubProcess_ComboBox.SelectedItem;

            if (pubKey_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定PubKey！";
                return;
            }
            Attribute pubKey = (Attribute)pubKey_ComboBox.SelectedItem;

            if (secProcess_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定SecProcess！";
                return;
            }
            Process secProcess = (Process)secProcess_ComboBox.SelectedItem;

            if (secKey_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定SecKey！";
                return;
            }
            Attribute secKey = (Attribute)secKey_ComboBox.SelectedItem;

            ObservableCollection<KeyPair> keyPairs = VM.InitialKnowledge.KeyPairs;
            // 判重
            foreach (KeyPair kp in keyPairs)
            {
                if (
                    kp.PubProcess == pubProcess &&
                    kp.PubKey == pubKey &&
                    kp.SecProcess == secProcess &&
                    kp.SecKey == secKey
                    )
                {
                    ResourceManager.mainWindowVM.Tips = "无效的操作。该公私钥对已经存在";
                    return;
                }
            }

            keyPair.PubProcess = pubProcess;
            keyPair.PubKey = pubKey;
            keyPair.SecProcess = secProcess;
            keyPair.SecKey = secKey;
            ResourceManager.mainWindowVM.Tips = "修改了KeyPair：" + keyPair;
        }

        public void Delete_KeyPair()
        {
            KeyPair keyPair = (KeyPair)keyPair_ListBox.SelectedItem;
            VM.InitialKnowledge.KeyPairs.Remove(keyPair);
            ResourceManager.mainWindowVM.Tips = "删除了KeyPair：" + keyPair;
        }

        #endregion

        #region 资源引用

        private ComboBox pubProcess_ComboBox, pubKey_ComboBox, secProcess_ComboBox, secKey_ComboBox;

        private ListBox keyPair_ListBox;

        public InitialKnowledge_EW_VM VM { get => ((InitialKnowledge_EW_VM)DataContext); }

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
