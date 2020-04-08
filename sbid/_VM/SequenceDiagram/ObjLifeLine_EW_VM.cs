using sbid._M;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    // 对象-生命线编辑的VM
    public class ObjLifeLine_EW_VM
    {
        private SeqObject seqObject;
        private ObservableCollection<Process> processes = new ObservableCollection<Process>();
        private bool safetyLock;
        private ObjLifeLine_VM objLifeLine_VM;

        // 要编辑的顺序图对象
        public SeqObject SeqObject { get => seqObject; set => seqObject = value; }
        // 集成协议下的所有Process
        public ObservableCollection<Process> Processes { get => processes; }
        // 安全锁，用于保护消息连线不在process_ComboBox选中项变化时被删除
        public bool SafetyLock { get => safetyLock; set => safetyLock = value; }
        // 也集成一下VM对象，这样才能获取它身上的锚点和连线
        // 仅在ObjLifeLine_EW_V.xaml.cs的process_ComboBox_Changed中有使用的需要
        public ObjLifeLine_VM ObjLifeLine_VM { get => objLifeLine_VM; set => objLifeLine_VM = value; }
    }
}
