using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.VisualTree;
using sbid._M;
using sbid._VM;
using SharpDX.WIC;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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

        // 无法直接获取到鼠标位置，必须在鼠标相关事件回调方法里取得
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            // 特别注意，要取得的不是相对这个ClassDiagram_P_V的位置，而是相对于里面的内容控件
            ItemsControl panel = ControlExtensions.FindControl<ItemsControl>(this, "panel");
            // 右键在这个面板上按下时
            if (e.GetCurrentPoint(panel).Properties.PointerUpdateKind == PointerUpdateKind.RightButtonPressed)
            {
                // 更新位置
                mousePos = e.GetPosition(panel);
            }
        }

        #endregion

        #region 右键菜单命令

        // 创建自定义类型
        public void CreateUserTypeVM()
        {
            UserType_VM userTypeVM = new UserType_VM() { X = mousePos.X, Y = mousePos.Y };

            // 确保UserType没有重名，检查有重名时就在后面随机跟字母
            UserType userType = (UserType)userTypeVM.Type;
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            System.Random random = new System.Random();
            while (Checker.UserType_Name_Repeat(userType, userType.Name))
            {
                userType.Name += letters[random.Next(52)];
            }

            VM.UserControlVMs.Add(userTypeVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的自定义类型：" + userTypeVM.Type.Name;
        }

        // 创建进程模板
        public void CreateProcessVM()
        {
            Process_VM processVM = new Process_VM() { X = mousePos.X, Y = mousePos.Y };

            // 确保Process没有重名，检查有重名时就在后面随机跟字母
            Process process = processVM.Process;
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            System.Random random = new System.Random();
            while (Checker.Process_Name_Repeat(process, process.RefName.Content))
            {
                process.RefName.Content += letters[random.Next(52)];
            }

            // 创建相应的"进程模板-状态机"大面板VM,并集成到当前Process_VM里
            processVM.ProcessToSM_P_VM = ResourceManager.mainWindowVM.AddProcessToSM(processVM.Process);

            VM.UserControlVMs.Add(processVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的进程模板：" + processVM.Process.RefName;
        }

        // 创建公理
        public void CreateAxiomVM()
        {
            Axiom_VM axiomVM = new Axiom_VM() { X = mousePos.X, Y = mousePos.Y };
            VM.UserControlVMs.Add(axiomVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的公理：" + axiomVM.Axiom.Name;
        }

        // 创建初始知识
        public void CreateInitialKnowledgeVM()
        {
            InitialKnowledge_VM initialKnowledgeVM = new InitialKnowledge_VM() { X = mousePos.X, Y = mousePos.Y };
            VM.UserControlVMs.Add(initialKnowledgeVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的InitialKnowledge";
        }

        // 创建功能安全性质
        public void CreateSafetyPropertyVM()
        {
            SafetyProperty_VM safetyPropertyVM = new SafetyProperty_VM() { X = mousePos.X, Y = mousePos.Y };
            VM.UserControlVMs.Add(safetyPropertyVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的SafetyProperty：" + safetyPropertyVM.SafetyProperty.Name;
        }

        // 创建信息安全性质
        public void CreateSecurityPropertyVM()
        {
            SecurityProperty_VM securityPropertyVM = new SecurityProperty_VM() { X = mousePos.X, Y = mousePos.Y };
            VM.UserControlVMs.Add(securityPropertyVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的SecurityProperty：" + securityPropertyVM.SecurityProperty.Name;
        }

        // 创建通信信道
        public void CreateCommChannelVM()
        {
            CommChannel_VM commChannelVM = new CommChannel_VM() { X = mousePos.X, Y = mousePos.Y };
            VM.UserControlVMs.Add(commChannelVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的CommChannel：" + commChannelVM.CommChannel.Name;
        }

        #endregion

        #region 按钮命令

        // 导出图片
        public async void ExportImage()
        {
            string path = await ResourceManager.GetSaveFileName("png");
            ItemsControl panel = ControlExtensions.FindControl<ItemsControl>(this, "panel");
            ResourceManager.RenderImage(path, panel);
        }

        #endregion

        // 对应的VM
        public ClassDiagram_P_VM VM { get => (ClassDiagram_P_VM)DataContext; }
    }
}
