using sbid._M;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    // 消息编辑窗体的VM
    public class Message_EW_VM
    {
        private CommMessage commMessage;
        private ObservableCollection<CommMethod> commMethods = new ObservableCollection<CommMethod>();

        // 要编辑的通信消息
        public CommMessage CommMessage { get => commMessage; set => commMessage = value; }
        // 所有可选的CommMethod，需要是Source方ObjLifeLine的进程模板的所有[OUT]型CommMethod
        public ObservableCollection<CommMethod> CommMethods { get => commMethods; }
    }
}
