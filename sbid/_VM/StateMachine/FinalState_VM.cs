using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class FinalState_VM : NetworkItem_VM
    {
        public FinalState_VM(double x, double y)
        {
            X = x;
            Y = y;
            ConnectorVMs = new ObservableCollection<Connector_VM>();
            ConnectorVMs.Add(new Connector_VM(X + 20, Y + 4) { NetworkItemVM = this });
        }

        #region 右键菜单

        public void DeleteFinalStateVM()
        {
            // 【注意】区分“状态机”和“访问控制”
            SidePanel_VM sidePanel_VM = ResourceManager.mainWindowVM.SelectedItem.SelectedItem.SelectedItem;
            if (sidePanel_VM is ProcessToSM_P_VM) // “状态机”
            {
                // 获取当前"进程模板-状态机"侧栏面板
                ProcessToSM_P_VM processToSM_P_VM = (ProcessToSM_P_VM)sidePanel_VM;
                // 获取当前的状态机面板
                StateMachine_P_VM stateMachine_P_VM = processToSM_P_VM.SelectedItem;
                // 删除状态上所有连线，并同时维护面板的活动锚点
                foreach (Connector_VM connector_VM in ConnectorVMs)
                {
                    // 如果活动锚点在这个要删除的状态上就会假死，所以要判断并清除活动锚点
                    if (connector_VM == stateMachine_P_VM.ActiveConnector)
                        stateMachine_P_VM.ActiveConnector = null;
                    // 清除该锚点上的连线，这里直接调用这个方法，即和用户手动点掉连线共享一样的行为
                    if (connector_VM.ConnectionVM != null)
                    {
                        stateMachine_P_VM.BreakTransitionVM(connector_VM);
                    }
                }
                // 从当前状态机面板删除这个状态
                stateMachine_P_VM.UserControlVMs.Remove(this);
                ResourceManager.mainWindowVM.Tips = "删除了终止状态";
            }
            else if (sidePanel_VM is AccessControl_P_VM) // “访问控制“
            {
                // 获取当前的访问控制面板
                AccessControl_P_VM accessControl_P_VM = (AccessControl_P_VM)sidePanel_VM;
                // 删除状态上所有连线，并同时维护面板的活动锚点
                foreach (Connector_VM connector_VM in ConnectorVMs)
                {
                    // 如果活动锚点在这个要删除的状态上就会假死，所以要判断并清除活动锚点
                    if (connector_VM == accessControl_P_VM.ActiveConnector)
                        accessControl_P_VM.ActiveConnector = null;
                    // 清除该锚点上的连线，这里直接调用这个方法，即和用户手动点掉连线共享一样的行为
                    if (connector_VM.ConnectionVM != null)
                    {
                        accessControl_P_VM.BreakTransitionVM(connector_VM);
                    }
                }
                // 从当前访问控制面板删除这个状态
                accessControl_P_VM.UserControlVMs.Remove(this);
                ResourceManager.mainWindowVM.Tips = "删除了终止状态";
            }
        }

        #endregion
    }
}
