using ReactiveUI;
using sbid._M;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class AttackTree_P_VM : SidePanel_VM
    {
        public static int _id = 0;
        private Connector_VM activeConnector;
        private Attack_VM handleAttackVM;
        private AttackWithRelation_VM handleAttackWithRelationVM;
        private ObservableCollection<Attack_VM> leafAttackVMs = new ObservableCollection<Attack_VM>();
        private ObservableCollection<AttackWithRelation_VM> leafAttackWithRelationVMs = new ObservableCollection<AttackWithRelation_VM>();
        private ObservableCollection<string> securityPolicies = new ObservableCollection<string>();
        private bool connectorVisible = true;
        private bool panelEnabled = true;

        // 默认构造时使用默认名称
        public AttackTree_P_VM()
        {
            _id++;
            this.refName = new Formula("攻击树" + _id);
        }

        // 活动锚点,当按下一个空闲锚点时,该锚点成为面板上唯一的活动锚点,当按下另一空闲锚点进行转移关系连线
        public Connector_VM ActiveConnector { get => activeConnector; set => activeConnector = value; }

        // 记录刚刚计算完的Attack_VM，用于应用[叶子攻击分析]的结果
        public Attack_VM HandleAttackVM { get => handleAttackVM; set => this.RaiseAndSetIfChanged(ref handleAttackVM, value); }
        // 【新】适合于AttackTree2目录的同一概念
        public AttackWithRelation_VM HandleAttackWithRelationVM { get => handleAttackWithRelationVM; set => this.RaiseAndSetIfChanged(ref handleAttackWithRelationVM, value); }

        // 用于[叶子攻击分析]的绑定列表
        public ObservableCollection<Attack_VM> LeafAttackVMs { get => leafAttackVMs; }
        // 【新】适合于AttackTree2目录的同一概念
        public ObservableCollection<AttackWithRelation_VM> LeafAttackWithRelationVMs { get => leafAttackWithRelationVMs; }

        // 用于[安全策略数据库]的绑定列表，这里绑定的只是和LeafAttackVMs的选中项匹配的安全策略字符串
        public ObservableCollection<string> SecurityPolicies { get => securityPolicies; }

        // 【作废】锚点是否可见
        public bool ConnectorVisible { get => connectorVisible; set => this.RaiseAndSetIfChanged(ref connectorVisible, value); }

        // 面板是否可用
        public bool PanelEnabled { get => panelEnabled; set => this.RaiseAndSetIfChanged(ref panelEnabled, value); }

        #region 按钮命令（作废）

        // 创建攻击结点
        public void CreateAttackVM()
        {
            Attack_VM attackVM = new Attack_VM(0, 0);
            UserControlVMs.Add(attackVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的攻击结点：" + attackVM.Attack.Content;
        }

        // 创建[与]关系
        public void CreateRelationVM_AND()
        {
            Relation_VM relationVM = new Relation_VM() { Relation = _M.Relation.AND };
            UserControlVMs.Add(relationVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的[与]关系结点(and)";
        }

        // 创建[或]关系
        public void CreateRelationVM_OR()
        {
            Relation_VM relationVM = new Relation_VM() { Relation = _M.Relation.OR };
            UserControlVMs.Add(relationVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的[或]关系结点(or)";
        }

        // 创建[非]关系
        public void CreateRelationVM_NEG()
        {
            Relation_VM relationVM = new Relation_VM() { Relation = _M.Relation.NEG };
            UserControlVMs.Add(relationVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的[非]关系结点(negation)";
        }

        // 创建[顺序与]关系
        public void CreateRelationVM_SAND()
        {
            Relation_VM relationVM = new Relation_VM() { Relation = _M.Relation.SAND };
            UserControlVMs.Add(relationVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的[顺序与]关系结点(sequence and)";
        }

        #endregion

        #region 攻击树上的VM操作接口

        /*
        // 创建树上连线关系（旧，适合于AttackTree目录的有箭头连线）
        public void CreateArrowVM(Connector_VM connectorVM1, Connector_VM connectorVM2)
        {
            Arrow_VM arrow_VM = new Arrow_VM();

            arrow_VM.Source = connectorVM1;
            arrow_VM.Dest = connectorVM2;

            // 锚点反引连接关系
            connectorVM1.ConnectionVM = arrow_VM;
            connectorVM2.ConnectionVM = arrow_VM;

            UserControlVMs.Add(arrow_VM);
        }
        */

        // 创建树上连线关系（新，适合于AttackTree2目录的无箭头连线）
        public void CreateArrowVM(Connector_VM connectorVM1, Connector_VM connectorVM2)
        {
            if (connectorVM1.NetworkItemVM == connectorVM2.NetworkItemVM)
            {
                ResourceManager.mainWindowVM.Tips = "不合法的连线！";
                return;
            }

            Connection_VM connection_VM = new Connection_VM();

            connection_VM.Source = connectorVM1;
            connection_VM.Dest = connectorVM2;

            // 锚点反引连接关系
            connectorVM1.ConnectionVM = connection_VM;
            connectorVM2.ConnectionVM = connection_VM;

            UserControlVMs.Add(connection_VM);

            ResourceManager.mainWindowVM.Tips = "创建了新的树上连线";
        }

        // 删除树上连线关系
        public void BreakArrowVM(Connector_VM connectorVM)
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
