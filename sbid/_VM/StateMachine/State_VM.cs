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

            #region 矩形参数
            /*
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
            */
            #endregion

            #region 椭圆参数

            // 椭圆中心位置
            double centerX = X + 65;
            double centerY = Y + 40;

            // 根据V中Grid的布局，计算横纵的一个单位的长度
            int colSum = 1 + 2 + 6 + 6 + 8 + 6 + 6 + 2 + 1;
            int rowSum = 1 + 1 + 2 + 3 + 3 + 3 + 2 + 1 + 1;
            double c = 120 / colSum + 0.2; // 用Width除以份数得到列最小单位
            double r = 70 / rowSum + 0.5; // 用Height除以份数得到行最小单位
            // 这里加的是一个误差修复值

            // 14个锚点,从椭圆中心位置进行位置推算，这里顺序和V中一样
            ConnectorVMs.Add(new Connector_VM(centerX - (4 + 6 + 3) * c, centerY - (1.5 + 3 + 1) * r));
            ConnectorVMs.Add(new Connector_VM(centerX - (4 + 3) * c, centerY - (1.5 + 3 + 2 + 0.5) * r));
            ConnectorVMs.Add(new Connector_VM(centerX, centerY - (1.5 + 3 + 2 + 1 + 1) * r));
            ConnectorVMs.Add(new Connector_VM(centerX + (4 + 3) * c, centerY - (1.5 + 3 + 2 + 0.5) * r));
            ConnectorVMs.Add(new Connector_VM(centerX + (4 + 6 + 3) * c, centerY - (1.5 + 3 + 1) * r));

            ConnectorVMs.Add(new Connector_VM(centerX - (4 + 6 + 6 + 1) * c, centerY - (1.5 + 1.5) * r));
            ConnectorVMs.Add(new Connector_VM(centerX + (4 + 6 + 6 + 1) * c, centerY - (1.5 + 1.5) * r));

            ConnectorVMs.Add(new Connector_VM(centerX - (4 + 6 + 6 + 1) * c, centerY + (1.5 + 1.5) * r));
            ConnectorVMs.Add(new Connector_VM(centerX + (4 + 6 + 6 + 1) * c, centerY + (1.5 + 1.5) * r));

            ConnectorVMs.Add(new Connector_VM(centerX - (4 + 6 + 3) * c, centerY + (1.5 + 3 + 1) * r));
            ConnectorVMs.Add(new Connector_VM(centerX - (4 + 3) * c, centerY + (1.5 + 3 + 2 + 0.5) * r));
            ConnectorVMs.Add(new Connector_VM(centerX, centerY + (1.5 + 3 + 2 + 1 + 1) * r));
            ConnectorVMs.Add(new Connector_VM(centerX + (4 + 3) * c, centerY + (1.5 + 3 + 2 + 0.5) * r));
            ConnectorVMs.Add(new Connector_VM(centerX + (4 + 6 + 3) * c, centerY + (1.5 + 3 + 1) * r));

            #endregion

            // 将这些锚点所在的NetworkItem_VM回引写入
            foreach (Connector_VM connector_VM in ConnectorVMs)
            {
                connector_VM.NetworkItemVM = this;
            }
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
            // 标记当前状态"被精化"，以显示蓝色标识
            state.HaveRefine = true;
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

        // 删除状态，级联删除对应内容
        public void DeleteStateVM()
        {
            // 获取当前"进程模板-状态机"侧栏面板
            ProcessToSM_P_VM processToSM_P_VM = (ProcessToSM_P_VM)ResourceManager.mainWindowVM.SelectedItem.PanelVMs[1].SelectedItem;
            // 获取当前的状态机面板
            StateMachine_P_VM stateMachine_P_VM = processToSM_P_VM.SelectedItem;
            // 删除状态上所有连线，并同时维护面板的活动锚点
            foreach (Connector_VM connector_VM in ConnectorVMs)
            {
                // 如果活动锚点在这个要删除的状态上就会假死，所以要判断并清除活动锚点
                if (connector_VM == stateMachine_P_VM.ActiveConnector)
                    stateMachine_P_VM.ActiveConnector = null;
                // 清除该锚点上的连线，这里直接调用这个方法，即和用户手动点掉连线共享一样的行为
                if (connector_VM.ConnectionVM != null)
                {
                    stateMachine_P_VM.BreakTransitionVM(connector_VM);
                }
                /*
                Connection_VM connection_VM = connector_VM.ConnectionVM;
                if (connection_VM != null)
                {
                    connection_VM.Source.ConnectionVM = null;
                    connection_VM.Dest.ConnectionVM = null;
                    stateMachine_P_VM.UserControlVMs.Remove(connection_VM);
                }
                */
            }

            // 判断并删除依赖此State的认证性/完整性/可用性
            JudgeAndDeleteProperty();

            // 删除对应状态机面板，以及递归删除其中的状态对应的状态机面板
            foreach (StateMachine_P_VM pvm in processToSM_P_VM.StateMachinePVMs)
            {
                if (pvm.State == state)
                {
                    DeleteStateMachinePVMCascade(pvm);
                    break; // 每个状态最多精化一次，所以找到一个面板删除完就可以break了
                }
            }

            // 从当前状态机面板删除这个状态
            stateMachine_P_VM.UserControlVMs.Remove(this);

            ResourceManager.mainWindowVM.Tips = "已经删除状态" + state.Name + "，并级联删除了所有依赖于此状态的内容";
        }

        #endregion

        #region 私有

        /// <summary>
        /// 判断并删除SecurityProperty中依赖此State的认证性/完整性，SafetyProperty中依赖此State的Availability
        /// </summary>
        /// <returns>是否做了删除操作</returns>
        public bool JudgeAndDeleteProperty()
        {
            bool deleted = false;
            // 当前协议面板VM
            Protocol_VM protocolVM = ResourceManager.mainWindowVM.SelectedItem;
            // 其下的类图面板VM
            ClassDiagram_P_VM classDiagram_P_VM = (ClassDiagram_P_VM)protocolVM.PanelVMs[0].SidePanelVMs[0];
            // 遍历查找SecurityProperty
            foreach (ViewModelBase item in classDiagram_P_VM.UserControlVMs)
            {
                if (item is SecurityProperty_VM)
                {
                    SecurityProperty_VM vm = (SecurityProperty_VM)item;
                    // 这里维护要删除的三种性质列表，遍历结束后再统一删除
                    List<Authenticity> authenticities = new List<Authenticity>();
                    List<Integrity> integrities = new List<Integrity>();
                    List<Availability> availabilities = new List<Availability>();
                    // 遍历查找
                    foreach (Authenticity authenticity in vm.SecurityProperty.Authenticities)
                    {
                        if (authenticity.StateA == state
                            || authenticity.StateB == state)
                        {
                            authenticities.Add(authenticity); // 加到待删除列表里
                        }
                    }
                    foreach (Integrity integrity in vm.SecurityProperty.Integrities)
                    {
                        if (integrity.StateA == state
                           || integrity.StateB == state)
                        {
                            integrities.Add(integrity);
                        }
                    }
                    foreach (Availability availability in vm.SecurityProperty.Availabilities)
                    {
                        if (availability.State == state)
                        {
                            availabilities.Add(availability);
                        }
                    }
                    // 统一删除
                    foreach (Authenticity authenticity in authenticities)
                    {
                        vm.SecurityProperty.Authenticities.Remove(authenticity);
                        deleted = true;
                    }
                    foreach (Integrity integrity in integrities)
                    {
                        vm.SecurityProperty.Integrities.Remove(integrity);
                        deleted = true;
                    }
                    foreach (Availability availability in availabilities)
                    {
                        vm.SecurityProperty.Availabilities.Remove(availability);
                        deleted = true;
                    }
                }
            }
            return deleted;
        }

        // 删除状态机面板，并递归删除其中的状态对应的状态机面板
        // 因为能删除的状态机面板一定不是顶层面板，面板的删除绝不会导致其中的状态被删除而影响Authenticity和Integrity
        public static void DeleteStateMachinePVMCascade(StateMachine_P_VM stateMachine_P_VM)
        {
            // 获取当前"进程模板-状态机"侧栏面板
            ProcessToSM_P_VM processToSM_P_VM = (ProcessToSM_P_VM)ResourceManager.mainWindowVM.SelectedItem.PanelVMs[1].SelectedItem;

            // 递归删除状态机面板中所有状态精化出的状态机面板
            // 先获取传入的状态机面板中的所有状态集合
            Collection<State> states = new Collection<State>();
            foreach (ViewModelBase viewModel in stateMachine_P_VM.UserControlVMs)
            {
                if (viewModel is State_VM)
                {
                    State_VM state_VM = ((State_VM)viewModel);
                    states.Add(state_VM.State);
                    // 这里也要检查并删除性质
                    state_VM.JudgeAndDeleteProperty();
                }
            }
            // 然后检查当前"进程模板-状态机"侧栏面板下的所有状态机面板，对记录下的状态对应的面板级联删除
            // 注意这里直接遍历processToSM_P_VM.StateMachinePVMs会导致迭代时删除，所以先记录到集合里去
            Collection<StateMachine_P_VM> needToDelete = new Collection<StateMachine_P_VM>();
            foreach (StateMachine_P_VM pvm in processToSM_P_VM.StateMachinePVMs)
            {
                if (states.Contains(pvm.State))
                {
                    needToDelete.Add(pvm);
                }
            }
            foreach (StateMachine_P_VM pvm in needToDelete)
            {
                DeleteStateMachinePVMCascade(pvm);
            }

            // 最后删除传入的状态机面板
            processToSM_P_VM.StateMachinePVMs.Remove(stateMachine_P_VM);
        }

        #endregion
    }
}
