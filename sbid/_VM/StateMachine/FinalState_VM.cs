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

        private void DeleteFinalStateVM()
        {
            // 获取当前"进程模板-状态机"侧栏面板
            ProcessToSM_P_VM processToSM_P_VM = (ProcessToSM_P_VM)ResourceManager.mainWindowVM.SelectedItem.PanelVMs[1].SelectedItem;
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

        #endregion
    }
}
