using Avalonia;
using sbid._M;
using sbid._V;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class StateTrans_VM : NetworkItem_VM
    {
        private StateTrans stateTrans = new StateTrans();

        public StateTrans_VM()
        {
            X = 50;
            Y = 50;
            ConnectorVMs = new ObservableCollection<Connector_VM>();

            // 左上角锚点中心位置
            double baseX = X + 4;
            double baseY = Y + 4;

            // 8个锚点，只需要设置好第一个，其它锚点可以后续通过它的位置配合H/W刷新确定
            ConnectorVMs.Add(new Connector_VM(baseX, baseY));
            for (int i = 0; i < 7; i++)
            {
                ConnectorVMs.Add(new Connector_VM());
            }
            // 将这些锚点所在的NetworkItem_VM回引写入
            foreach (Connector_VM connector_VM in ConnectorVMs)
            {
                connector_VM.NetworkItemVM = this;
            }
        }

        // 构造时添加8个锚点
        public StateTrans_VM(double x, double y)
        {
            X = x;
            Y = y;
            ConnectorVMs = new ObservableCollection<Connector_VM>();

            // 左上角锚点中心位置
            double baseX = X + 4;
            double baseY = Y + 4;

            // 8个锚点，只需要设置好第一个，其它锚点可以后续通过它的位置配合H/W刷新确定
            ConnectorVMs.Add(new Connector_VM(baseX, baseY));
            for (int i = 0; i < 7; i++)
            {
                ConnectorVMs.Add(new Connector_VM());
            }
            // 将这些锚点所在的NetworkItem_VM回引写入
            foreach (Connector_VM connector_VM in ConnectorVMs)
            {
                connector_VM.NetworkItemVM = this;
            }
        }

        public StateTrans StateTrans { get => stateTrans; }

        #region 右键菜单命令

        // 打开编辑窗口
        public void EditStateTrans()
        {
            // 【注意】区分“状态机”和“访问控制”
            SidePanel_VM sidePanel_VM = ResourceManager.mainWindowVM.SelectedItem.SelectedItem.SelectedItem;
            if (sidePanel_VM is ProcessToSM_P_VM) // “状态机”
            {
                // 从所在的ProcessToSM面板取得当前状态机所在的进程模板
                ProcessToSM_P_VM processToSM_P_VM = (ProcessToSM_P_VM)sidePanel_VM;

                // 窗体VM
                StateTrans_EW_VM stateTrans_EW_VM = new StateTrans_EW_VM()
                {
                    StateTrans = stateTrans,
                    Process = processToSM_P_VM.Process
                };

                // 从进程模板生成属性导航
                foreach (Attribute attribute in processToSM_P_VM.Process.Attributes)
                {
                    Nav nav;
                    if (attribute.Type is UserType) // 引用类型
                    {
                        nav = ReferenceNav.build(attribute, null);
                    }
                    else // 值类型
                    {
                        nav = new ValueNav(attribute, null);
                    }
                    stateTrans_EW_VM.Properties.Add(nav);
                }

                // 窗体V
                StateTrans_EW_V stateTransEWV = new StateTrans_EW_V()
                {
                    DataContext = stateTrans_EW_VM
                };

                // 将所有的Type传入，作为原子命题编辑窗口中属性导航器的可选进程
                foreach (ViewModelBase item in ResourceManager.mainWindowVM.SelectedItem.PanelVMs[0].SidePanelVMs[0].UserControlVMs)
                {
                    if (item is UserType_VM)
                    {
                        UserType_VM userType_VM = (UserType_VM)item;
                        ((StateTrans_EW_VM)stateTransEWV.DataContext).Types.Add(userType_VM.Type);
                    }
                }

                stateTransEWV.ShowDialog(ResourceManager.mainWindowV);
                ResourceManager.mainWindowVM.Tips = "打开了转移关系的编辑窗体";

                // 【bugfix】打开后，禁用当前窗体
                StateMachine_P_VM stateMachineP_VM = processToSM_P_VM.SelectedItem;
                stateMachineP_VM.PanelEnabled = false;
            }
            else if (sidePanel_VM is AccessControl_P_VM) // “访问控制”
            {
                // 窗体VM
                StateTrans_EW_VM stateTrans_EW_VM = new StateTrans_EW_VM()
                {
                    StateTrans = stateTrans,
                    Process = null
                };

                // 窗体V
                StateTrans_EW_V stateTransEWV = new StateTrans_EW_V()
                {
                    DataContext = stateTrans_EW_VM
                };

                // 将所有的Type传入，作为原子命题编辑窗口中属性导航器的可选进程
                foreach (ViewModelBase item in ResourceManager.mainWindowVM.SelectedItem.PanelVMs[0].SidePanelVMs[0].UserControlVMs)
                {
                    if (item is UserType_VM)
                    {
                        UserType_VM userType_VM = (UserType_VM)item;
                        ((StateTrans_EW_VM)stateTransEWV.DataContext).Types.Add(userType_VM.Type);
                    }
                }

                stateTransEWV.ShowDialog(ResourceManager.mainWindowV);
                ResourceManager.mainWindowVM.Tips = "打开了转移关系的编辑窗体";

                // 【bugfix】打开后，禁用当前窗体
                AccessControl_P_VM accessControl_P_VM = (AccessControl_P_VM)sidePanel_VM;
                accessControl_P_VM.PanelEnabled = false;
            }
        }

        // 删除这个StateTrans
        public void DeleteStateTrans()
        {
            // 【注意】区分“状态机”和“访问控制”
            SidePanel_VM sidePanel_VM = ResourceManager.mainWindowVM.SelectedItem.SelectedItem.SelectedItem;
            if (sidePanel_VM is ProcessToSM_P_VM) // “状态机”
            {
                // 获取当前"进程模板-状态机"侧栏面板
                ProcessToSM_P_VM processToSM_P_VM = (ProcessToSM_P_VM)sidePanel_VM;
                // 获取当前的状态机面板
                StateMachine_P_VM stateMachine_P_VM = processToSM_P_VM.SelectedItem;
                // 删除状态上所有连线，并同时维护面板的活动锚点
                foreach (Connector_VM connector_VM in ConnectorVMs)
                {
                    // 如果活动锚点在这个要删除的StateTrans上就会假死，所以要判断并清除活动锚点
                    if (connector_VM == stateMachine_P_VM.ActiveConnector)
                        stateMachine_P_VM.ActiveConnector = null;
                    // 清除该锚点上的连线，这里直接调用这个方法，即和用户手动点掉连线共享一样的行为
                    if (connector_VM.ConnectionVM != null)
                    {
                        stateMachine_P_VM.BreakTransitionVM(connector_VM);
                    }
                }

                // 从当前状态机面板删除这个StateTrans
                stateMachine_P_VM.UserControlVMs.Remove(this);

                ResourceManager.mainWindowVM.Tips = "已经删除状态转移";
            }
            else if (sidePanel_VM is AccessControl_P_VM) // “访问控制”
            {
                // 获取当前的访问控制面板
                AccessControl_P_VM accessControl_P_VM = (AccessControl_P_VM)sidePanel_VM;
                // 删除状态上所有连线，并同时维护面板的活动锚点
                foreach (Connector_VM connector_VM in ConnectorVMs)
                {
                    // 如果活动锚点在这个要删除的StateTrans上就会假死，所以要判断并清除活动锚点
                    if (connector_VM == accessControl_P_VM.ActiveConnector)
                        accessControl_P_VM.ActiveConnector = null;
                    // 清除该锚点上的连线，这里直接调用这个方法，即和用户手动点掉连线共享一样的行为
                    if (connector_VM.ConnectionVM != null)
                    {
                        accessControl_P_VM.BreakTransitionVM(connector_VM);
                    }
                }

                // 从当前访问控制面板删除这个StateTrans
                accessControl_P_VM.UserControlVMs.Remove(this);

                ResourceManager.mainWindowVM.Tips = "已经删除状态转移";
            }
        }

        #endregion

        // 刷新锚点位置
        public new void FlushConnectorPos()
        {
            // 左上角锚点位置
            double x0 = ConnectorVMs[0].Pos.X;
            double y0 = ConnectorVMs[0].Pos.Y;
            // 一个一个确定
            Point pos = new Point(x0 + W / 2, y0);
            ConnectorVMs[1].Pos = pos;

            pos = new Point(x0 + W, y0);
            ConnectorVMs[2].Pos = pos;

            pos = new Point(x0, y0 + H / 2);
            ConnectorVMs[3].Pos = pos;

            pos = new Point(x0 + W, y0 + H / 2);
            ConnectorVMs[4].Pos = pos;

            pos = new Point(x0, y0 + H);
            ConnectorVMs[5].Pos = pos;

            pos = new Point(x0 + W / 2, y0 + H);
            ConnectorVMs[6].Pos = pos;

            pos = new Point(x0 + W, y0 + H);
            ConnectorVMs[7].Pos = pos;
        }
    }
}
