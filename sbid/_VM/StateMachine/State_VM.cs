using sbid._M;
using sbid._V;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    // 普通状态VM
    public class State_VM : NetworkItem_VM
    {
        private State state = new State();

        // 构造时添加6个锚点
        public State_VM(double x, double y)
        {
            X = x;
            Y = y;
            ConnectorVMs = new ObservableCollection<Connector_VM>();

            // 左上角锚点中心位置
            double baseX = X + 6;
            double baseY = Y + 8;
            // 横纵方向锚点间距
            double deltaX = 24.5;
            double deltaY = 16;

            // 14个锚点,从左上角锚点中心位置进行位置推算
            ConnectorVMs.Add(new Connector_VM(baseX + 0 * deltaX, baseY + 0 * deltaY));
            ConnectorVMs.Add(new Connector_VM(baseX + 1 * deltaX, baseY + 0 * deltaY));
            ConnectorVMs.Add(new Connector_VM(baseX + 2 * deltaX, baseY + 0 * deltaY));
            ConnectorVMs.Add(new Connector_VM(baseX + 3 * deltaX, baseY + 0 * deltaY));
            ConnectorVMs.Add(new Connector_VM(baseX + 4 * deltaX, baseY + 0 * deltaY));

            ConnectorVMs.Add(new Connector_VM(baseX + 0 * deltaX, baseY + 1 * deltaY));
            ConnectorVMs.Add(new Connector_VM(baseX + 4 * deltaX, baseY + 1 * deltaY));

            ConnectorVMs.Add(new Connector_VM(baseX + 0 * deltaX, baseY + 2 * deltaY));
            ConnectorVMs.Add(new Connector_VM(baseX + 4 * deltaX, baseY + 2 * deltaY));

            ConnectorVMs.Add(new Connector_VM(baseX + 0 * deltaX, baseY + 3 * deltaY));
            ConnectorVMs.Add(new Connector_VM(baseX + 1 * deltaX, baseY + 3 * deltaY));
            ConnectorVMs.Add(new Connector_VM(baseX + 2 * deltaX, baseY + 3 * deltaY));
            ConnectorVMs.Add(new Connector_VM(baseX + 3 * deltaX, baseY + 3 * deltaY));
            ConnectorVMs.Add(new Connector_VM(baseX + 4 * deltaX, baseY + 3 * deltaY));
        }

        public State State { get => state; set => state = value; }

        #region 右键菜单命令

        // 尝试打开编辑状态结点的窗口
        public void EditStateVM()
        {
            // 从主窗体打开编辑窗体,并在其DataContext中集成当前State_VM里集成的State对象,以能对其作修改
            State_EW_V stateEWV = new State_EW_V()
            {
                DataContext = new State_EW_VM()
                {
                    State = state
                }
            };
            stateEWV.ShowDialog(ResourceManager.mainWindowV);
            ResourceManager.mainWindowVM.Tips = "打开了状态[" + state.Name + "]的编辑窗体";
        }

        // 对状态进行精化(跳转到精化的状态机面板)
        public void RefineStateVM()
        {
            // 获取当前"进程模板-状态机"侧栏面板
            ProcessToSM_P_VM processToSM_P_VM = (ProcessToSM_P_VM)ResourceManager.mainWindowVM.SelectedItem.PanelVMs[1].SelectedItem;
            // 判断是否已经精化过，如果精化过直接跳到面板
            foreach (StateMachine_P_VM pvm in processToSM_P_VM.StateMachinePVMs)
            {
                if (pvm.State == state)
                {
                    processToSM_P_VM.SelectedItem = pvm;
                    return;
                }
            }
            // 否则，创建、初始化并跳转
            StateMachine_P_VM stateMachine_P_VM = new StateMachine_P_VM(state);
            stateMachine_P_VM.init_data();
            processToSM_P_VM.StateMachinePVMs.Add(stateMachine_P_VM);
            processToSM_P_VM.SelectedItem = stateMachine_P_VM;
        }

        #endregion
    }
}
