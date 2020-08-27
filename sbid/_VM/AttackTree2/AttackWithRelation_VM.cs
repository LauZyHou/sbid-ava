using Avalonia.Media;
using ReactiveUI;
using sbid._M;
using sbid._V;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace sbid._VM
{
    public class AttackWithRelation_VM : NetworkItem_VM
    {
        #region 属性

        public static int _id = 0;
        private AttackWithRelation attackWithRelation;
        private bool beAttacked = false;
        private bool isLocked = false;

        // 攻击和关系内容
        public AttackWithRelation AttackWithRelation { get => attackWithRelation; }
        // 攻击结点取值，表示是否受攻击
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
        // 结点颜色,受取值影响
        public ISolidColorBrush NodeColor
        {
            get
            {
                return beAttacked ? Brushes.LightPink : Brushes.LightGreen;
            }
        }

        #endregion

        #region 构造

        // 仅用于xaml设计
        public AttackWithRelation_VM()
        {
            _id++;
            attackWithRelation = new AttackWithRelation("Attack" + _id);
        }

        // 实际使用这个构造
        public AttackWithRelation_VM(double x, double y)
        {
            _id++;
            attackWithRelation = new AttackWithRelation("Attack" + _id);

            X = x;
            Y = y;

            // 左上角位置
            double baseX = X + 6;
            double baseY = Y + 8;

            // 横向锚点间距
            double deltaX = 20;
            // 高度
            double height = 80;

            init_connector(baseX, baseY, deltaX, height);
        }

        // 补充构造：初始化所有的锚点
        private void init_connector(double baseX, double baseY, double deltaX, double height)
        {
            ConnectorVMs = new ObservableCollection<Connector_VM>();

            ConnectorVMs.Add(new Connector_VM(baseX + 50, baseY));

            // 最下面一排baseX要补上一个偏移量
            baseX += 10;

            ConnectorVMs.Add(new Connector_VM(baseX + 0 * deltaX, baseY + height));
            ConnectorVMs.Add(new Connector_VM(baseX + 1 * deltaX, baseY + height));
            ConnectorVMs.Add(new Connector_VM(baseX + 2 * deltaX, baseY + height));
            ConnectorVMs.Add(new Connector_VM(baseX + 3 * deltaX, baseY + height));
            ConnectorVMs.Add(new Connector_VM(baseX + 4 * deltaX, baseY + height));

            // 将这些锚点所在的NetworkItem_VM回引写入
            foreach (Connector_VM connector_VM in ConnectorVMs)
            {
                connector_VM.NetworkItemVM = this;
            }
        }

        #endregion

        #region 右键菜单命令

        // 打开编辑窗体
        private void OpenEditWindow()
        {
            AttackWithRelation_EW_V attackWithRelation_EW_V = new AttackWithRelation_EW_V()
            {
                DataContext = new AttackWithRelation_EW_VM()
                {
                    AttackWithRelation = attackWithRelation
                }
            };
            attackWithRelation_EW_V.ShowDialog(ResourceManager.mainWindowV);
            ResourceManager.mainWindowVM.Tips = "打开了攻击树结点：" + attackWithRelation.Description + "的编辑窗体";
        }

        // 反转结点取值
        private void ReverseBeAttacked()
        {
            BeAttacked = !beAttacked;
            ResourceManager.mainWindowVM.Tips = beAttacked ? "修改结点为受攻击(True)" : "修改结点为安全(False)";
        }

        // 攻击树该结点为根节点，计算BeAttacked的值
        public void CalculateBeAttacked()
        {
            // 当前的攻击树面板
            AttackTree_P_VM attackTree_P_VM = (AttackTree_P_VM)ResourceManager.mainWindowVM.SelectedItem.PanelVMs[3].SelectedItem;

            // 记录当前计算的这个攻击树结点
            attackTree_P_VM.HandleAttackWithRelationVM = this;

            // 移除[叶子攻击分析]结果列表
            attackTree_P_VM.LeafAttackWithRelationVMs.Clear();

            // todo 判断是一棵树

            // 以此为根递归求值
            recursive_eval(this);

            // 如果最终是可攻击的，要找到导致这些攻击的源头
            if (beAttacked)
            {
                // 递归寻找可疑的叶子结点列表
                List<AttackWithRelation_VM> ret = find_leaf_attack(this);
                foreach (AttackWithRelation_VM attackWithRelation_VM in ret)
                {
                    attackTree_P_VM.LeafAttackWithRelationVMs.Add(attackWithRelation_VM);
                }
            }

            ResourceManager.mainWindowVM.Tips = "计算完成，结点[" + attackWithRelation.Description + "]是" + (beAttacked ? "可攻击" : "安全") + "的";
        }

        // 删除该攻击树结点
        private void DeleteAttackWithRelationVM()
        {
            AttackTree_P_VM attackTree_P_VM = (AttackTree_P_VM)ResourceManager.mainWindowVM.SelectedItem.PanelVMs[3].SelectedItem;
            if (attackTree_P_VM.ActiveConnector != null)
            {
                attackTree_P_VM.ActiveConnector.IsActive = false;
                attackTree_P_VM.ActiveConnector = null;
            }
            Utils.deleteAndClearNetworkItemVM(this, attackTree_P_VM);
            ResourceManager.mainWindowVM.Tips = "删除了攻击树结点：" + attackWithRelation.Description;
        }

        #endregion

        #region 静态

        /// <summary>
        /// 寻找可能使当前结点受攻击的叶子结点
        /// </summary>
        /// <param name="rootNode">当前子树根的VM</param>
        /// <returns>找到的叶子结点列表</returns>
        private static List<AttackWithRelation_VM> find_leaf_attack(AttackWithRelation_VM rootNode)
        {
            // 因为该函数仅当在发生攻击时才调用，所以传入该函数的一定都是红的结点(BeAttacked=true)
            // 由此，递归出口（锁定结点和实际叶子）都一定是所求范围内的结点
            if (rootNode.isLocked)
            {
                return new List<AttackWithRelation_VM> { rootNode };
            }
            // 先找到所有孩子结点
            List<AttackWithRelation_VM> childrenNodes = new List<AttackWithRelation_VM>();
            for (int i = 1; i <= 5; i++) // 跳过第一个，第一个是指向父节点的
            {
                NetworkItem_VM child = Utils.getAnotherEndNetWorkItemVM(rootNode.ConnectorVMs[i]);
                if (child != null)
                {
                    childrenNodes.Add(child as AttackWithRelation_VM);
                }
            }
            // 如果没有孩子结点，说明是叶子
            if (childrenNodes.Count == 0)
            {
                return new List<AttackWithRelation_VM> { rootNode };
            }
            // 否则，根据当前的AttackRelation和所有子树的取值来计算
            List<AttackWithRelation_VM> ret = new List<AttackWithRelation_VM>();
            switch (rootNode.AttackWithRelation.AttackRelation)
            {
                // 或关系被攻击，则一定有孩子为true，将它们计算进来
                case AttackRelation.OR:
                    foreach (AttackWithRelation_VM child in childrenNodes)
                    {
                        if (child.beAttacked)
                        {
                            ret = ret.Union(find_leaf_attack(child)).ToList();
                        }
                    }
                    break;
                // 与关系被攻击，则所有孩子肯定都是true，都要计算进来
                case AttackRelation.AND:
                case AttackRelation.SAND:
                    foreach (AttackWithRelation_VM child in childrenNodes)
                    {
                        ret = ret.Union(find_leaf_attack(child)).ToList();
                    }
                    break;
                default:
                    break;
            }
            return ret;
        }

        /// <summary>
        /// 攻击树结点递归求值
        /// </summary>
        /// <param name="rootNode">当前子树根的VM</param>
        /// <returns>求值结果</returns>
        private static bool recursive_eval(AttackWithRelation_VM rootNode)
        {
            // 如果是锁定的，直接返回值
            if (rootNode.isLocked)
            {
                return rootNode.beAttacked;
            }
            // 先找到所有孩子结点
            List<AttackWithRelation_VM> childrenNodes = new List<AttackWithRelation_VM>();
            for (int i = 1; i <= 5; i++) // 跳过第一个，第一个是指向父节点的
            {
                NetworkItem_VM child = Utils.getAnotherEndNetWorkItemVM(rootNode.ConnectorVMs[i]);
                if (child != null)
                {
                    childrenNodes.Add(child as AttackWithRelation_VM);
                }
            }
            // 如果没有孩子结点，说明是叶子，直接返回值
            if (childrenNodes.Count == 0)
            {
                return rootNode.beAttacked;
            }
            // 否则，根据当前的AttackRelation和所有子树的取值来计算
            bool val = false;
            switch (rootNode.AttackWithRelation.AttackRelation)
            {
                case AttackRelation.OR:
                    val = false;
                    foreach (AttackWithRelation_VM child in childrenNodes)
                    {
                        val |= recursive_eval(child);
                    }
                    break;
                case AttackRelation.AND:
                case AttackRelation.SAND:
                    val = true;
                    foreach (AttackWithRelation_VM child in childrenNodes)
                    {
                        val &= recursive_eval(child);
                    }
                    break;
                default:
                    break;
            }
            // 在递归计算退栈时候就沿途把取值写入结点里
            rootNode.BeAttacked = val;
            return val;
        }

        #endregion
    }
}
