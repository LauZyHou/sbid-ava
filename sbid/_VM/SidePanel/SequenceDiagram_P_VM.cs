using ReactiveUI;
using sbid._M;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class SequenceDiagram_P_VM : SidePanel_VM
    {
        public static int _id = 0;
        private Connector_VM activeConnector;
        private SeqMessage seqMessage = SeqMessage.SyncMessage;
        private bool connectorVisible = true;

        // 默认构造时使用默认名称
        public SequenceDiagram_P_VM()
        {
            _id++;
            this.refName = new Formula("序列图" + _id);
        }

        // 活动锚点,当按下一个空闲锚点时,该锚点成为面板上唯一的活动锚点,当按下另一空闲锚点进行转移关系连线
        public Connector_VM ActiveConnector { get => activeConnector; set => activeConnector = value; }
        // 当前选中的SeqMessage
        public SeqMessage SeqMessage { get => seqMessage; set => this.RaiseAndSetIfChanged(ref seqMessage, value); }
        // 锚点是否可见
        public bool ConnectorVisible { get => connectorVisible; set => this.RaiseAndSetIfChanged(ref connectorVisible, value); }

        #region 按钮命令

        // 创建对象-生命线
        public void CreateObjLifeLineVM()
        {
            ObjLifeLine_VM objLifeLineVM = new ObjLifeLine_VM(50, 50);
            UserControlVMs.Add(objLifeLineVM);
            ResourceManager.mainWindowVM.Tips = "添加了对象-生命线";
        }

        #endregion

        #region 顺序图上的VM操作接口

        // 创建Message传递关系（此函数在"_V"下的"MessageConnector_V"中调用）
        public void CreateSeqMessageVM(Connector_VM connectorVM1, Connector_VM connectorVM2)
        {
            // 根据SeqMessage枚举的不同来创建不同的Message_VM实例
            Message_VM messageVM;
            switch (seqMessage)
            {
                case SeqMessage.SyncMessage:
                    messageVM = new SyncMessage_VM();
                    break;
                case SeqMessage.AsyncMessage:
                    messageVM = new AsyncMessage_VM();
                    break;
                case SeqMessage.ReturnMessage:
                    messageVM = new ReturnMessage_VM();
                    break;
                case SeqMessage.SyncMessage_Self:
                    messageVM = new SyncMessage_Self_VM();
                    break;
                case SeqMessage.AsyncMessage_Self:
                    messageVM = new AsyncMessage_Self_VM();
                    break;
                default:
                    ResourceManager.mainWindowVM.Tips = "[ERROR]发生在SequenceDiagram_P_VM.cs";
                    return;
            }

            messageVM.Source = connectorVM1;
            messageVM.Dest = connectorVM2;

            // 锚点反引连接关系
            connectorVM1.ConnectionVM = messageVM;
            connectorVM2.ConnectionVM = messageVM;

            UserControlVMs.Add(messageVM);
        }

        // 删除锚点上的Message传递关系
        public void BreakSeqMessageVM(Connector_VM connectorVM)
        {
            // 要删除的转移关系
            Connection_VM connectionVM = connectorVM.ConnectionVM;

            // 从图形上移除
            UserControlVMs.Remove(connectionVM);

            // 找转移关系的两端锚点(有一个是自己,但是不用管哪个是自己)
            Connector_VM source = connectionVM.Source;
            Connector_VM dest = connectionVM.Dest;

            // 清除反引
            source.ConnectionVM = dest.ConnectionVM = null;
        }

        #endregion
    }
}
