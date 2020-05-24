using sbid._M;
using sbid._V;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class Process_VM : NetworkItem_VM
    {
        private Process process;
        private ProcessToSM_P_VM processToSM_P_VM;

        public Process_VM()
        {
            process = new Process();
        }

        public Process Process { get => process; set => process = value; }
        // 集成当前Process对应的状态机的面板VM
        public ProcessToSM_P_VM ProcessToSM_P_VM { get => processToSM_P_VM; set => processToSM_P_VM = value; }


        #region 右键菜单命令

        // 尝试删除当前Process_VM
        public void DeleteProcessVM()
        {
            Protocol_VM nowProtocolPanel = ResourceManager.mainWindowVM.SelectedItem;
            nowProtocolPanel.SelectedItem.SelectedItem.UserControlVMs.Remove(this);

            // 将对应状态机面板也删除
            nowProtocolPanel.PanelVMs[1].SidePanelVMs.Remove(processToSM_P_VM);

            // 判断并删除SecurityProperty中依赖此Process的保密性/认证性/完整性
            bool del = JudgeAndDeleteSecurityPropertyAttr();

            ResourceManager.mainWindowVM.Tips = "删除了进程模板：" + process.RefName + "及对应状态机。";

            // Tips补充
            if (del)
            {
                ResourceManager.mainWindowVM.Tips += "[!]删除了SecurityProperty中依赖此Process的保密性/认证性/完整性。";
            }
        }

        // 查看当前Process_VM对应的状态机
        public void FindProcessToSMPVM()
        {
            Protocol_VM nowProtocolPanel = ResourceManager.mainWindowVM.SelectedItem;
            // 跳到"协议>状态机"选项卡
            nowProtocolPanel.SelectedItem = nowProtocolPanel.PanelVMs[1];
            // 选中当前进程对应的ProcessToSM_P_VM
            nowProtocolPanel.PanelVMs[1].SelectedItem = processToSM_P_VM;
            // 并且选中其第一个状态机(顶层状态机)
            processToSM_P_VM.SelectedItem = processToSM_P_VM.StateMachinePVMs[0];
        }

        // 打开当前Process_VM的编辑窗体
        public void EditProcessVM()
        {
            // 从主窗体打开编辑窗体,并在其DataContext中集成当前Process_VM里集成的Process对象,以能对其作修改
            Process_EW_V processEWV = new Process_EW_V()
            {
                DataContext = new Process_EW_VM()
                {
                    Process = process
                }
            };
            // 将所有的Type也传入,作为Attribute/Method/CommMethod的可用类型
            foreach (ViewModelBase item in ResourceManager.mainWindowVM.SelectedItem.SelectedItem.SelectedItem.UserControlVMs)
            {
                if (item is UserType_VM)
                {
                    ((Process_EW_VM)processEWV.DataContext).Types.Add(((UserType_VM)item).Type);
                }
            }

            processEWV.ShowDialog(ResourceManager.mainWindowV);
            ResourceManager.mainWindowVM.Tips = "打开了进程模板：" + process.RefName + "的编辑窗体";
        }

        #endregion

        #region 私有

        /// <summary>
        /// 判断并删除SecurityProperty中依赖此Process的保密性/认证性/完整性
        /// </summary>
        /// <returns>是否做了删除操作</returns>
        public bool JudgeAndDeleteSecurityPropertyAttr()
        {
            bool deleted = false;
            // 当前协议面板VM
            Protocol_VM protocolVM = ResourceManager.mainWindowVM.SelectedItem;
            // 其下的类图面板VM
            ClassDiagram_P_VM classDiagram_P_VM = (ClassDiagram_P_VM)protocolVM.PanelVMs[0].SidePanelVMs[0];
            // 遍历查找SecurityProperty
            foreach (ViewModelBase item in classDiagram_P_VM.UserControlVMs)
            {
                if (item is SecurityProperty_VM)
                {
                    SecurityProperty_VM vm = (SecurityProperty_VM)item;
                    // 这里维护要删除的三种性质列表，遍历结束后再统一删除
                    List<Confidential> confidentials = new List<Confidential>();
                    List<Authenticity> authenticities = new List<Authenticity>();
                    List<Integrity> integrities = new List<Integrity>();
                    // 遍历查找
                    foreach (Confidential confidential in vm.SecurityProperty.Confidentials)
                    {
                        if (confidential.Process == process)
                        {
                            confidentials.Add(confidential);
                        }
                    }
                    foreach (Authenticity authenticity in vm.SecurityProperty.Authenticities)
                    {
                        if (authenticity.ProcessA == process
                            || authenticity.ProcessB == process)
                        {
                            authenticities.Add(authenticity); // 加到待删除列表里
                        }
                    }
                    foreach (Integrity integrity in vm.SecurityProperty.Integrities)
                    {
                        if (integrity.ProcessA == process
                           || integrity.ProcessB == process)
                        {
                            integrities.Add(integrity);
                        }
                    }
                    // 统一删除
                    foreach (Confidential confidential in confidentials)
                    {
                        vm.SecurityProperty.Confidentials.Remove(confidential);
                        deleted = true;
                    }
                    foreach (Authenticity authenticity in authenticities)
                    {
                        vm.SecurityProperty.Authenticities.Remove(authenticity);
                        deleted = true;
                    }
                    foreach (Integrity integrity in integrities)
                    {
                        vm.SecurityProperty.Integrities.Remove(integrity);
                        deleted = true;
                    }
                }
            }
            return deleted;
        }

        #endregion
    }
}
