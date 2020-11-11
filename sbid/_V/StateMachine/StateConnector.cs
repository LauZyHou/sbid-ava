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

            // 【注意】区分“状态机”和“访问控制”
            SidePanel_VM sidePanel_VM = ResourceManager.mainWindowVM.SelectedItem.SelectedItem.SelectedItem;
            if (sidePanel_VM is ProcessToSM_P_VM) // “状态机”
            {
                StateMachine_P_VM nowSMPVM = ((ProcessToSM_P_VM)sidePanel_VM).SelectedItem;
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
            else if (sidePanel_VM is AccessControl_P_VM) // “访问控制”
            {
                AccessControl_P_VM nowSMPVM = (AccessControl_P_VM)sidePanel_VM;
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
}
