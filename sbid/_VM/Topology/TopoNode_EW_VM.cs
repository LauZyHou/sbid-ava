﻿using sbid._M;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class TopoNode_EW_VM
    {
        private TopoNode topoNode;
        private ObservableCollection<Process> processes = new ObservableCollection<Process>();
        private bool safetyLock;

        // 要编辑的TopoNode
        public TopoNode TopoNode { get => topoNode; set => topoNode = value; }
        // 集成协议下的所有Process
        public ObservableCollection<Process> Processes { get => processes; set => processes = value; }
        // 安全锁，用于保护例化对象不在process_ComboBox选中项变化时被重新生成
        public bool SafetyLock { get => safetyLock; set => safetyLock = value; }
    }
}