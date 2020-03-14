using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using sbid._VM;

namespace sbid._V
{
    public class ClassDiagram_P_V : Network_P_V
    {
        public ClassDiagram_P_V()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #region 监听鼠标位置用

        private Point mousePos;

        // 无法直接获取到鼠标位置，必须在这个事件回调方法里取得
        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);
            // 特别注意，要取得的不是相对这个ClassDiagram_P_V的位置，而是相对于里面的内容控件
            ItemsControl panel = ControlExtensions.FindControl<ItemsControl>(this, "panel");
            mousePos = e.GetPosition(panel);
        }

        #endregion

        #region 右键菜单命令

        // 创建自定义类型
        public void CreateUserTypeVM()
        {
            UserType_VM userTypeVM = new UserType_VM() { X = mousePos.X, Y = mousePos.Y };
            ClassDiagramPVM.NetworkItemVMs.Add(userTypeVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的自定义类型：" + userTypeVM.Type.Name;
        }

        // 创建进程模板
        public void CreateProcessVM()
        {
            Process_VM processVM = new Process_VM() { X = mousePos.X, Y = mousePos.Y };

            // 创建相应的状态机,并集成到当前Process_VM里
            processVM.StateMachine_P_VM = ResourceManager.mainWindowVM.AddStateMachine(processVM.Process);

            ClassDiagramPVM.NetworkItemVMs.Add(processVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的进程模板：" + processVM.Process.Name;
        }

        // 创建公理
        public void CreateAxiomVM()
        {
            Axiom_VM axiomVM = new Axiom_VM() { X = mousePos.X, Y = mousePos.Y };
            ClassDiagramPVM.NetworkItemVMs.Add(axiomVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的公理：" + axiomVM.Axiom.Name;
        }

        // 创建初始知识
        public void CreateInitialKnowledgeVM()
        {
            InitialKnowledge_VM initialKnowledgeVM = new InitialKnowledge_VM() { X = mousePos.X, Y = mousePos.Y };
            ClassDiagramPVM.NetworkItemVMs.Add(initialKnowledgeVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的InitialKnowledge：" + initialKnowledgeVM.InitialKnowledge.Name;
        }

        // 创建功能安全性质
        public void CreateSafetyPropertyVM()
        {
            SafetyProperty_VM safetyPropertyVM = new SafetyProperty_VM() { X = mousePos.X, Y = mousePos.Y };
            ClassDiagramPVM.NetworkItemVMs.Add(safetyPropertyVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的SafetyProperty：" + safetyPropertyVM.SafetyProperty.Name;
        }

        // 创建信息安全性质
        public void CreateSecurityPropertyVM()
        {
            SecurityProperty_VM securityPropertyVM = new SecurityProperty_VM() { X = mousePos.X, Y = mousePos.Y };
            ClassDiagramPVM.NetworkItemVMs.Add(securityPropertyVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的SecurityProperty：" + securityPropertyVM.SecurityProperty.Name;
        }

        // 创建通信信道
        public void CreateCommChannelVM()
        {
            CommChannel_VM commChannelVM = new CommChannel_VM() { X = mousePos.X, Y = mousePos.Y };
            ClassDiagramPVM.NetworkItemVMs.Add(commChannelVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的CommChannel：" + commChannelVM.CommChannel.Name;
        }

        #endregion

        // 对应的VM
        public ClassDiagram_P_VM ClassDiagramPVM { get => (ClassDiagram_P_VM)DataContext; }
    }
}
