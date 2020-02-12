using Avalonia.Input;
using sbid._VM;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._V
{
    // 顺序图中消息的连线
    public class MessageConnector_V : Connector_V
    {
        // 鼠标按下
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            // 获取当前顺序图面板
            SequenceDiagram_P_VM nowSDPVM = (SequenceDiagram_P_VM)ResourceManager.mainWindowVM.SelectedItem.SelectedItem.SelectedItem;

            // 如果有连线,那么要做的是删除连线
            if (ConnectorVM.ConnectionVM != null)
            {
                nowSDPVM.BreakSeqMessageVM(ConnectorVM);
                ResourceManager.mainWindowVM.Tips = "删除了消息传递关系";
            }
            // 如果已经是活动锚点,要清除活动状态
            else if (nowSDPVM.ActiveConnector == ConnectorVM) // 也可用ConnectorVM.IsActive
            {
                ConnectorVM.IsActive = false;
                nowSDPVM.ActiveConnector = null;
                ResourceManager.mainWindowVM.Tips = "清除了锚点的预连线状态";
            }
            // 否则,即是空闲锚点
            // 如果还没有活动锚点,设置其为活动锚点
            else if (nowSDPVM.ActiveConnector == null)
            {
                nowSDPVM.ActiveConnector = ConnectorVM;
                ConnectorVM.IsActive = true;
                ResourceManager.mainWindowVM.Tips = "设置锚点为预连线锚点";
            }
            // 至此,说明已经有一个活动锚点了,且当前锚点是另一个空闲锚点,这时要从活动锚点连线到此锚点
            else
            {
                // 创建时要传入SeqMessage类型
                nowSDPVM.CreateSeqMessageVM(nowSDPVM.ActiveConnector, ConnectorVM, nowSDPVM.SeqMessage);
                // 清除活动锚点
                nowSDPVM.ActiveConnector.IsActive = false;
                nowSDPVM.ActiveConnector = null;
                ResourceManager.mainWindowVM.Tips = "创建了新的" + nowSDPVM.SeqMessage + "传递关系";
            }

            e.Handled = true;
        }
    }
}
