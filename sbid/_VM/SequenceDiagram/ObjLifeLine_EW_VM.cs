using sbid._M;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class ObjLifeLine_EW_VM
    {
        private SeqObject seqObject;
        private ObservableCollection<Process> processes = new ObservableCollection<Process>();
        private bool safetyLock;

        // 要编辑的顺序图对象
        public SeqObject SeqObject { get => seqObject; set => seqObject = value; }
        // 集成协议下的所有Process
        public ObservableCollection<Process> Processes { get => processes; set => processes = value; }
        // 安全锁，用于保护消息连线不在process_ComboBox选中项变化时被删除
        public bool SafetyLock { get => safetyLock; set => safetyLock = value; }
    }
}
