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
        private StateMachine_P_VM stateMachine_P_VM;

        public Process_VM()
        {
            process = new Process();
        }

        public Process Process { get => process; set => process = value; }
        // 集成当前Process对应的状态机的面板VM
        public StateMachine_P_VM StateMachine_P_VM { get => stateMachine_P_VM; set => stateMachine_P_VM = value; }

        #region 右键菜单命令

        // 尝试删除当前Process_VM
        public void DeleteProcessVM()
        {
            Protocol_VM nowProtocolPanel = ResourceManager.mainWindowVM.SelectedItem;
            nowProtocolPanel.SelectedItem.SelectedItem.NetworkItemVMs.Remove(this);

            // 将对应状态机面板也删除
            nowProtocolPanel.PanelVMs[1].SidePanelVMs.Remove(stateMachine_P_VM);

            ResourceManager.mainWindowVM.Tips = "删除了进程模板：" + process.Name + "及对应状态机";
        }

        // 查看当前Process_VM对应的状态机
        public void FindStateMachinePVM()
        {
            Protocol_VM nowProtocolPanel = ResourceManager.mainWindowVM.SelectedItem;
            nowProtocolPanel.SelectedItem = nowProtocolPanel.PanelVMs[1];
            nowProtocolPanel.PanelVMs[1].SelectedItem = stateMachine_P_VM;
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
            foreach (NetworkItem_VM item in ResourceManager.mainWindowVM.SelectedItem.SelectedItem.SelectedItem.NetworkItemVMs)
            {
                if (item is UserType_VM)
                {
                    ((Process_EW_VM)processEWV.DataContext).Types.Add(((UserType_VM)item).Type);
                }
            }

            processEWV.ShowDialog(ResourceManager.mainWindowV);
            ResourceManager.mainWindowVM.Tips = "打开了进程模板：" + process.Name + "的编辑窗体";
        }

        #endregion
    }
}
