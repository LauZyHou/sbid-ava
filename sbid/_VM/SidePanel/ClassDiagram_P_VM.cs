using System;
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
            Name = "类图";
            init_data();
        }

        private void init_data()
        {
            NetworkItemVMs.Add(
                new UserType_VM(sbid._M.Type.TYPE_INT)
                {
                    X = 1000,
                    Y = 20
                });
            NetworkItemVMs.Add(
                new UserType_VM(sbid._M.Type.TYPE_BOOL)
                {
                    X = 970,
                    Y = 50
                });
        }

        #region 按钮和右键菜单命令

        // 创建自定义类型
        public void CreateUserTypeVM()
        {
            UserType_VM userTypeVM = new UserType_VM();
            NetworkItemVMs.Add(userTypeVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的自定义类型：" + userTypeVM.Type.Name;
        }

        // 创建进程模板
        public void CreateProcessVM()
        {
            Process_VM processVM = new Process_VM();

            // 创建相应的状态机,并集成到当前Process_VM里
            processVM.StateMachine_P_VM = ResourceManager.mainWindowVM.AddStateMachine(processVM.Process);

            NetworkItemVMs.Add(processVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的进程模板：" + processVM.Process.Name;
        }

        // 创建公理
        public void CreateAxiomVM()
        {
            Axiom_VM axiomVM = new Axiom_VM();
            NetworkItemVMs.Add(axiomVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的公理：" + axiomVM.Axiom.Name;
        }
        
        // 创建初始知识
        public void CreateInitialKnowledgeVM()
        {
            InitialKnowledge_VM initialKnowledgeVM = new InitialKnowledge_VM();
            NetworkItemVMs.Add(initialKnowledgeVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的InitialKnowledge：" + initialKnowledgeVM.InitialKnowledge.Name;
        }

        // 创建功能安全性质
        public void CreateSafetyPropertyVM()
        {
            SafetyProperty_VM safetyPropertyVM = new SafetyProperty_VM();
            NetworkItemVMs.Add(safetyPropertyVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的SafetyProperty：" + safetyPropertyVM.SafetyProperty.Name;
        }

        #endregion
    }
}
