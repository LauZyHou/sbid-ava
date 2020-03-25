using Avalonia.Media;
using ReactiveUI;
using sbid._M;
using sbid._V;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace sbid._VM
{
    public class Attack_VM : NetworkItem_VM
    {
        private static int _id = 1;
        private Attack attack;
        private bool beAttacked = false;
        private bool isLocked = false;


        public Attack_VM()
        {
            attack = new Attack("Attack" + _id);
            _id++;

            X = 0;
            Y = 0;

            // 左上角锚点中心位置
            double baseX = X + 6;
            double baseY = Y + 8;
            // 横纵方向锚点间距
            double deltaX = 24.5;
            double deltaY = 16;

            init_connector(baseX, baseY, deltaX, deltaY);
        }

        public Attack_VM(double x, double y)
        {
            attack = new Attack("Attack" + _id);
            _id++;

            X = x;
            Y = y;

            // 左上角锚点中心位置
            double baseX = X + 6;
            double baseY = Y + 8;
            // 横纵方向锚点间距
            double deltaX = 24.5;
            double deltaY = 16;

            init_connector(baseX, baseY, deltaX, deltaY);
        }

        // 攻击结点上的攻击
        public Attack Attack { get => attack; set => attack = value; }
        // 攻击结点取值,true表示攻击会发生,false表示不会发生
        public bool BeAttacked
        {
            get => beAttacked;
            set
            {
                this.RaiseAndSetIfChanged(ref beAttacked, value);
                this.RaisePropertyChanged("NodeColor");
            }
        }
        // 指示结点取值是否被锁定
        public bool IsLocked { get => isLocked; set => this.RaiseAndSetIfChanged(ref isLocked, value); }
        // 边框颜色,受取值影响
        public ISolidColorBrush NodeColor
        {
            get
            {
                return beAttacked ? Brushes.LightPink : Brushes.LightGreen;
            }
        }

        #region 按钮和右键菜单命令

        // 尝试打开编辑当前攻击结点的窗体
        public void EditAttackVM()
        {
            // 从主窗体打开编辑窗体,并在其DataContext中集成当前Attack_VM里集成的Attack对象,以能对其作修改
            Attack_EW_V attackEWV = new Attack_EW_V()
            {
                DataContext = new Attack_EW_VM()
                {
                    Attack = attack
                }
            };
            attackEWV.ShowDialog(ResourceManager.mainWindowV);
            ResourceManager.mainWindowVM.Tips = "打开了攻击结点：" + attack.Content + "的编辑窗体";
        }

        // 反转结点取值
        public void ReverseBeAttacked()
        {
            BeAttacked = !beAttacked;
            ResourceManager.mainWindowVM.Tips = beAttacked ? "修改结点为受攻击(True)" : "修改结点为安全(False)";
        }

        // 从孩子结点计算BeAttacked的值
        public void CalculateBeAttacked()
        {
            // 当前的攻击树面板
            AttackTree_P_VM attackTree_P_VM = (AttackTree_P_VM)ResourceManager.mainWindowVM.SelectedItem.SelectedItem.SelectedItem;

            /*
             todo:通过孩子结点计算并设置当前结点BeAttacked的值是true(可攻击)还是false(安全)
             其中SAND关系就直接当作AND关系处理
             需要注意,递归到锁定(IsLocked=true)的结点就要退栈了,那样的结点直接使用其BeAttacked的值,而不继续向下到叶子
             */

            // 在sbid-ava中，一棵合法的攻击树满足下列条件
            // 1. 是一个有向无环图
            // 2. 每一个攻击结点的孩子只能是一个关系结点(常见)或一个攻击结点(不常见)
            // 3. 关系结点的孩子只能是攻击结点，并且关系结点不能作为叶子
            // 4. OR/AND/SAND至少有一个孩子，NEG必须有且只有一个孩子

            // 实现上的细节
            // 5. 关系结点理解成一种布尔值的聚合操作，所以只能在攻击结点上调用该函数求值
            // 6. 递归求值时，出口是叶子攻击结点或锁定的攻击结点
            // 7. 在求值退栈过程中要将沿途的结点设定出计算后的值

            // todo 以此结点为root，检查是否是合法的攻击树

            // 移除[叶子攻击分析]结果列表
            attackTree_P_VM.LeafAttackVMs.Clear();

            // 递归求值
            recursive_eval(this);

            // 如果最终是可攻击的，要找到导致这些攻击的源头
            if (beAttacked)
            {
                // 递归寻找可疑的叶子结点列表
                List<Attack_VM> ret = find_leaf_attack(this);
                foreach (Attack_VM attack_VM in ret)
                {
                    attackTree_P_VM.LeafAttackVMs.Add(attack_VM);
                }
            }

            ResourceManager.mainWindowVM.Tips = "计算完成，该结点是" + (beAttacked ? "可攻击" : "安全") + "的";
        }

        #endregion

        #region 私有方法

        // 补充构造：初始化所有的锚点
        private void init_connector(double baseX, double baseY, double deltaX, double deltaY)
        {
            ConnectorVMs = new ObservableCollection<Connector_VM>();

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

            // 将这些锚点所在的NetworkItem_VM回引写入
            foreach (Connector_VM connector_VM in ConnectorVMs)
            {
                connector_VM.NetworkItemVM = this;
            }
        }

        // 检查是否是叶子，即周围的锚点是否都没有流入的
        private bool is_leaf()
        {
            foreach (Connector_VM connector_VM in this.ConnectorVMs)
            {
                if (connector_VM.ConnectionVM != null && connector_VM.ConnectionVM.Dest == connector_VM)
                {
                    return false;
                }
            }
            return true;
        }

        // 对攻击结点递归求值
        private static bool recursive_eval(Attack_VM avm)
        {
            // 递归出口
            if (avm.isLocked || avm.is_leaf())
                return avm.beAttacked;
            // 不是叶子，那么有且仅有一个锚点上连线的Dest是当前Attack_VM avm的这个锚点
            // 通过这个锚点上的连线，来找到Source端的孩子结点
            NetworkItem_VM child_VM = null; // 可能是Relation_VM或者Attack_VM
            foreach (Connector_VM connector_VM in avm.ConnectorVMs)
            {
                // 流入的
                if (connector_VM.ConnectionVM != null && connector_VM.ConnectionVM.Dest == connector_VM)
                {
                    child_VM = connector_VM.ConnectionVM.Source.NetworkItemVM;
                    break;
                }
            }
            // 肯定能找到一个的，因为已经在递归前检查过了
            Debug.Assert(child_VM != null);

            // 当前结点求值结果。它一定会在下面的if-else中被重新赋值，所以这里初始值不重要
            bool val = false;

            // 如果唯一孩子是攻击结点
            if (child_VM is Attack_VM)
            {
                val = recursive_eval(child_VM as Attack_VM);
            }

            // 如果唯一孩子是关系结点
            else if (child_VM is Relation_VM)
            {
                Relation_VM relation_VM = child_VM as Relation_VM;
                // 记录流入这个Relation_VM的所有Attack_VM
                List<Attack_VM> attackInList = new List<Attack_VM>();
                foreach (Connector_VM connector_VM in relation_VM.ConnectorVMs)
                {
                    // 流入的
                    if (connector_VM.ConnectionVM != null && connector_VM.ConnectionVM.Dest == connector_VM)
                    {
                        attackInList.Add((Attack_VM)connector_VM.ConnectionVM.Source.NetworkItemVM); // 递归前检查过了类型
                    }
                }
                Debug.Assert(attackInList.Count >= 1); // Relation_VM不能作为叶子，所以一定有流入的Attack_VM
                switch (relation_VM.Relation)
                {
                    case Relation.AND:
                    case Relation.SAND:
                        val = true; // 与关系的"单位元"
                        foreach (Attack_VM attackIn in attackInList)
                        {
                            val &= recursive_eval(attackIn);
                        }
                        break;
                    case Relation.OR:
                        val = false; // 或关系的"单位元"
                        foreach (Attack_VM attackIn in attackInList)
                        {
                            val |= recursive_eval(attackIn);
                        }
                        break;
                    case Relation.NEG:
                        val = !recursive_eval(attackInList[0]);
                        break;
                    default:
                        break;
                }
            }

            avm.BeAttacked = val;
            return val;
        }

        // 从当前结点寻找叶子(因为NEG的存在，当前结点不一定是BeAttacked=true的)
        // 实际上目标就是找到可能使avm取值翻转的叶子结点
        private static List<Attack_VM> find_leaf_attack(Attack_VM avm)
        {
            // 递归出口
            if (avm.isLocked || avm.is_leaf())
                return new List<Attack_VM> { avm };
            // 不是叶子，那么有且仅有一个锚点上连线的Dest是当前Attack_VM avm的这个锚点
            // 通过这个锚点上的连线，来找到Source端的孩子结点
            NetworkItem_VM child_VM = null; // 可能是Relation_VM或者Attack_VM
            foreach (Connector_VM connector_VM in avm.ConnectorVMs)
            {
                // 流入的
                if (connector_VM.ConnectionVM != null && connector_VM.ConnectionVM.Dest == connector_VM)
                {
                    child_VM = connector_VM.ConnectionVM.Source.NetworkItemVM;
                    break;
                }
            }

            // 如果唯一孩子是攻击结点
            if (child_VM is Attack_VM)
            {
                return find_leaf_attack(child_VM as Attack_VM);
            }

            // 如果唯一孩子是关系结点
            else if (child_VM is Relation_VM)
            {
                Relation_VM relation_VM = child_VM as Relation_VM;
                // 记录流入这个Relation_VM的所有Attack_VM
                List<Attack_VM> attackInList = new List<Attack_VM>();
                foreach (Connector_VM connector_VM in relation_VM.ConnectorVMs)
                {
                    // 流入的
                    if (connector_VM.ConnectionVM != null && connector_VM.ConnectionVM.Dest == connector_VM)
                    {
                        attackInList.Add((Attack_VM)connector_VM.ConnectionVM.Source.NetworkItemVM); // 递归前检查过了类型
                    }
                }
                // 考虑关系类型来在返回的结果List中添加成员
                List<Attack_VM> ret = new List<Attack_VM>();
                switch (relation_VM.Relation)
                {
                    case Relation.AND:
                    case Relation.SAND:
                        if (avm.beAttacked == true) // 如果使当前结点受攻击，那么所有孩子结点都有贡献
                        {
                            foreach (Attack_VM attackIn in attackInList)
                            {
                                ret = ret.Union(find_leaf_attack(attackIn)).ToList();
                            }
                        }
                        else // 如果当前结点安全，那么安全的孩子结点有贡献
                        {
                            foreach (Attack_VM attackIn in attackInList)
                            {
                                if (attackIn.beAttacked == false)
                                {
                                    ret = ret.Union(find_leaf_attack(attackIn)).ToList();
                                }
                            }
                        }
                        break;
                    case Relation.OR:
                        if (avm.beAttacked == true) // 如果使当前结点受攻击，那么受攻击的孩子结点有贡献
                        {
                            foreach (Attack_VM attackIn in attackInList)
                            {
                                if (attackIn.beAttacked == true)
                                {
                                    ret = ret.Union(find_leaf_attack(attackIn)).ToList();
                                }
                            }
                        }
                        else // 如果当前结点安全，那么所有孩子结点都有贡献
                        {
                            foreach (Attack_VM attackIn in attackInList)
                            {
                                ret = ret.Union(find_leaf_attack(attackIn)).ToList();
                            }
                        }
                        break;
                    case Relation.NEG:
                        // NEG反倒不用特判，因为一定是让这个唯一孩子反转
                        ret = ret.Union(find_leaf_attack(attackInList[0])).ToList();
                        break;
                    default:
                        break;
                }
                return ret;
            }

            // 不会执行到这里
            return new List<Attack_VM>();
        }

        #endregion
    }
}
