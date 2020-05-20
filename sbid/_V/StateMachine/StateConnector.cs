using Avalonia.Input;
using sbid._VM;

namespace sbid._V
{
    public class StateConnector_V : Connector_V
    {
        // 鼠标按下
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            // 获取当前状态机面板
            ProcessToSM_P_VM processToSM_P_VM = (ProcessToSM_P_VM)ResourceManager.mainWindowVM.SelectedItem.PanelVMs[1].SelectedItem;
            StateMachine_P_VM nowSMPVM = processToSM_P_VM.SelectedItem;

            // 如果有连线,那么要做的是删除连线
            if (ConnectorVM.ConnectionVM != null)
            {
                nowSMPVM.BreakTransitionVM(ConnectorVM);
                ResourceManager.mainWindowVM.Tips = "删除了转移关系";
            }
            // 如果已经是活动锚点,要清除活动状态
            else if (nowSMPVM.ActiveConnector == ConnectorVM) // 也可用ConnectorVM.IsActive
            {
                ConnectorVM.IsActive = false;
                nowSMPVM.ActiveConnector = null;
                ResourceManager.mainWindowVM.Tips = "清除了锚点的预连线状态";
            }
            // 否则,即是空闲锚点
            // 如果还没有活动锚点,设置其为活动锚点
            else if (nowSMPVM.ActiveConnector == null)
            {
                nowSMPVM.ActiveConnector = ConnectorVM;
                ConnectorVM.IsActive = true;
                ResourceManager.mainWindowVM.Tips = "设置锚点为预连线锚点";
            }
            // 至此,说明已经有一个活动锚点了,且当前锚点是另一个空闲锚点,这时要从活动锚点连线到此锚点
            else
            {
                nowSMPVM.CreateTransitionVM(nowSMPVM.ActiveConnector, ConnectorVM);
                // 清除活动锚点
                nowSMPVM.ActiveConnector.IsActive = false;
                nowSMPVM.ActiveConnector = null;
                ResourceManager.mainWindowVM.Tips = "创建了新的状态转移关系";
            }

            e.Handled = true;
        }
    }
}
