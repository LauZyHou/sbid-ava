using sbid._M;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    // 类图面板
    public class ClassDiagram_P_VM : SidePanel_VM
    {
        public ClassDiagram_P_VM()
        {
            this.refName = new Formula("类图");
        }

        // 这里改成给外部主动调用
        public void init_data()
        {
            double baseX = 20;
            double baseY = 20;
            double deltaY = 30;
            int count = 0;

            UserControlVMs.Add(
                new UserType_VM(Type.TYPE_INT)
                {
                    X = baseX,
                    Y = baseY + (count++) * deltaY
                });
            UserControlVMs.Add(
                new UserType_VM(Type.TYPE_BOOL)
                {
                    X = baseX,
                    Y = baseY + (count++) * deltaY
                });
            UserControlVMs.Add(
                new UserType_VM(Type.TYPE_NUM)
                {
                    X = baseX,
                    Y = baseY + (count++) * deltaY
                });
            UserControlVMs.Add(
                new UserType_VM(Type.TYPE_BYTE)
                {
                    X = baseX,
                    Y = baseY + (count++) * deltaY
                });
            // 这里额外添加Timer类和ByteVec类，但它们不是内置类型
            UserControlVMs.Add(
                new UserType_VM(Type.TYPE_BYTE_VEC)
                {
                    X = baseX + 160,
                    Y = baseY
                });
            UserControlVMs.Add(
                new UserType_VM(Type.TYPE_TIMER)
                {
                    X = baseX + 160,
                    Y = baseY + deltaY + 10
                });
        }

        #region 按钮命令

        // 注意右键菜单命令因为要使用鼠标位置，所以放到ClassDiagram_P_V.xaml.cs里了
        // 如果下面的内容有改动，那里也要修改

        // 创建自定义类型
        public void CreateUserTypeVM()
        {
            UserType_VM userTypeVM = new UserType_VM();
            UserControlVMs.Add(userTypeVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的自定义类型：" + userTypeVM.Type.Name;
        }

        // 创建进程模板
        public void CreateProcessVM()
        {
            Process_VM processVM = new Process_VM();

            // 创建相应的状态机,并集成到当前Process_VM里
            processVM.StateMachine_P_VM = ResourceManager.mainWindowVM.AddStateMachine(processVM.Process);

            UserControlVMs.Add(processVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的进程模板：" + processVM.Process.RefName;
        }

        // 创建公理
        public void CreateAxiomVM()
        {
            Axiom_VM axiomVM = new Axiom_VM();
            UserControlVMs.Add(axiomVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的公理：" + axiomVM.Axiom.Name;
        }

        // 创建初始知识
        public void CreateInitialKnowledgeVM()
        {
            InitialKnowledge_VM initialKnowledgeVM = new InitialKnowledge_VM();
            UserControlVMs.Add(initialKnowledgeVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的InitialKnowledge";
        }

        // 创建功能安全性质
        public void CreateSafetyPropertyVM()
        {
            SafetyProperty_VM safetyPropertyVM = new SafetyProperty_VM();
            UserControlVMs.Add(safetyPropertyVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的SafetyProperty：" + safetyPropertyVM.SafetyProperty.Name;
        }

        // 创建信息安全性质
        public void CreateSecurityPropertyVM()
        {
            SecurityProperty_VM securityPropertyVM = new SecurityProperty_VM();
            UserControlVMs.Add(securityPropertyVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的SecurityProperty：" + securityPropertyVM.SecurityProperty.Name;
        }

        // 创建通信信道
        public void CreateCommChannelVM()
        {
            CommChannel_VM commChannelVM = new CommChannel_VM();
            UserControlVMs.Add(commChannelVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的CommChannel：" + commChannelVM.CommChannel.Name;
        }

        #endregion
    }
}
