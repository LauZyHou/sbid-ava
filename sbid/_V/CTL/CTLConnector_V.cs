using Avalonia.Input;
using sbid._VM;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._V
{
    public class CTLConnector_V : Connector_V
    {
        // 鼠标按下
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            // 获取当前CTL面板
            CTLTree_P_VM nowCTLPVM = (CTLTree_P_VM)ResourceManager.mainWindowVM.SelectedItem.SelectedItem.SelectedItem;

            // 如果有连线,那么要做的是删除连线
            if (ConnectorVM.ConnectionVM != null)
            {
                nowCTLPVM.BreakArrowVM(ConnectorVM);
                ResourceManager.mainWindowVM.Tips = "删除了树上连线";
            }
            // 如果已经是活动锚点,要清除活动状态
            else if (nowCTLPVM.ActiveConnector == ConnectorVM) // 也可用ConnectorVM.IsActive
            {
                ConnectorVM.IsActive = false;
                nowCTLPVM.ActiveConnector = null;
                ResourceManager.mainWindowVM.Tips = "清除了锚点的预连线状态";
            }
            // 否则,即是空闲锚点
            // 如果还没有活动锚点,设置其为活动锚点
            else if (nowCTLPVM.ActiveConnector == null)
            {
                nowCTLPVM.ActiveConnector = ConnectorVM;
                ConnectorVM.IsActive = true;
                ResourceManager.mainWindowVM.Tips = "设置锚点为预连线锚点";
            }
            // 至此,说明已经有一个活动锚点了,且当前锚点是另一个空闲锚点,这时要从活动锚点连线到此锚点
            else
            {
                nowCTLPVM.CreateArrowVM(nowCTLPVM.ActiveConnector, ConnectorVM);
                // 清除活动锚点
                nowCTLPVM.ActiveConnector.IsActive = false;
                nowCTLPVM.ActiveConnector = null;
                ResourceManager.mainWindowVM.Tips = "创建了新的树上连线";
            }

            e.Handled = true;
        }
    }
}
