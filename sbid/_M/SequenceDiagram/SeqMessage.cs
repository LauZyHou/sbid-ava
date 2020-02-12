using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // 顺序图的消息类型:同步消息,异步消息,返回消息,同步消息(到自己),异步消息(到自己)
    public enum SeqMessage
    {
        SyncMessage, AsyncMessage, ReturnMessage, SyncMessage_Self, AsyncMessage_Self
    }
}
