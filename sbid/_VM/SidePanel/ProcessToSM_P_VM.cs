using ReactiveUI;
using sbid._M;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class ProcessToSM_P_VM : SidePanel_VM
    {
        private Process process;
        private ObservableCollection<StateMachine_P_VM> stateMachinePVMs = new ObservableCollection<StateMachine_P_VM>();
        private StateMachine_P_VM selectedItem;

        // 无参构造只是给xaml中的Design用
        public ProcessToSM_P_VM()
        {
        }

        public ProcessToSM_P_VM(Process process)
        {
            this.process = process;
            this.refName = process.RefName;
        }

        // 集成所在Process,以反向查询(以用其Name)
        public Process Process { get => process; set => process = value; }
        // 集成所有的状态机面板
        public ObservableCollection<StateMachine_P_VM> StateMachinePVMs { get => stateMachinePVMs; set => stateMachinePVMs = value; }
        // 选中的状态机
        public StateMachine_P_VM SelectedItem { get => selectedItem; set => this.RaiseAndSetIfChanged(ref selectedItem, value); }

        #region 对外的初始化调用(在用户创建时需要调用，在从项目文件读取时不可调用)

        public void init_data()
        {
            // 创建默认的顶层状态机面板
            StateMachine_P_VM stateMachine_P_VM = new StateMachine_P_VM(State.TopState);
            stateMachine_P_VM.init_data();
            // 加到列表里
            stateMachinePVMs.Add(stateMachine_P_VM);
            SelectedItem = stateMachine_P_VM;
        }

        #endregion
    }
}
