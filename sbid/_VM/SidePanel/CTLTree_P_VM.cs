using sbid._M;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class CTLTree_P_VM : SidePanel_VM
    {
        public static int _id = 0;
        private Connector_VM activeConnector;
        private Formula cTLFormula = new Formula("");

        // 默认构造时使用默认名称
        public CTLTree_P_VM()
        {
            _id++;
            this.refName = new Formula("CTL树" + _id);
        }

        // 活动锚点,当按下一个空闲锚点时,该锚点成为面板上唯一的活动锚点,当按下另一空闲锚点进行转移关系连线
        public Connector_VM ActiveConnector { get => activeConnector; set => activeConnector = value; }

        public Formula CTLFormula { get => cTLFormula; }

        #region CTL树上的VM操作接口

        // 创建树上连线关系
        public void CreateArrowVM(Connector_VM connectorVM1, Connector_VM connectorVM2)
        {
            // 单向用Connection，双向用Arrow，CTL语法树用单向就可以了
            Connection_VM arrow_VM = new Connection_VM();

            arrow_VM.Source = connectorVM1;
            arrow_VM.Dest = connectorVM2;

            // 锚点反引连接关系
            connectorVM1.ConnectionVM = arrow_VM;
            connectorVM2.ConnectionVM = arrow_VM;

            UserControlVMs.Add(arrow_VM);
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

        #region 静态

        // 递归计算CTL子树的CTL公式，还要传入是否检查加括号
        public static string recursive_eval(NetworkItem_VM node, bool checkParen = false)
        {
            if (node is CTLRelation_VM) // CTL关系结点
            {
                CTLRelation_VM crvm = (CTLRelation_VM)node;
                switch (crvm.CTLRelation)
                {
                    case CTLRelation.AX:
                        return eval_one("AX", node);
                    case CTLRelation.EX:
                        return eval_one("EX", node);
                    case CTLRelation.AF:
                        return eval_one("AF", node);
                    case CTLRelation.EF:
                        return eval_one("EF", node);
                    case CTLRelation.AG:
                        return eval_one("AG", node);
                    case CTLRelation.EG:
                        return eval_one("EG", node);
                    case CTLRelation.AU:
                        return eval_bin("AU", node);
                    case CTLRelation.EU:
                        return eval_bin("EU", node);
                }
            }
            else if (node is LogicRelation_VM) // 命题逻辑关系结点
            {
                LogicRelation_VM lrvm = (LogicRelation_VM)node;
                switch (lrvm.LogicRelation)
                {
                    case LogicRelation.CONJ:
                        string ans = eval_bin("∧", node);
                        return checkParen ? "(" + ans + ")" : ans;
                    case LogicRelation.DISJ:
                        ans = eval_bin("∨", node);
                        return checkParen ? "(" + ans + ")" : ans;
                    case LogicRelation.NEG:
                        return eval_one("¬", node);
                    case LogicRelation.IMPL:
                        ans = eval_bin("→", node);
                        return checkParen ? "(" + ans + ")" : ans;
                }
            }
            else if (node is AtomProposition_VM) // 原子命题
            {
                AtomProposition_VM apvm = (AtomProposition_VM)node;
                return apvm.AtomProposition.RefName.Content;
            }
            return "ERROR"; // node == null
        }

        // 给定关系字符串str，在二元关系结点上计算相应的公式
        public static string eval_bin(string str, NetworkItem_VM node)
        {
            NetworkItem_VM leftNode, rightNode;
            // 左子
            if (node.ConnectorVMs[3].ConnectionVM == null)
            {
                leftNode = null;
            }
            else if (node.ConnectorVMs[3].ConnectionVM.Dest == node.ConnectorVMs[3])
            {
                leftNode = node.ConnectorVMs[3].ConnectionVM.Source.NetworkItemVM;
            }
            else
            {
                leftNode = node.ConnectorVMs[3].ConnectionVM.Dest.NetworkItemVM;
            }
            // 右子
            if (node.ConnectorVMs[1].ConnectionVM == null)
            {
                rightNode = null;
            }
            else if (node.ConnectorVMs[1].ConnectionVM.Dest == node.ConnectorVMs[1])
            {
                rightNode = node.ConnectorVMs[1].ConnectionVM.Source.NetworkItemVM;
            }
            else
            {
                rightNode = node.ConnectorVMs[1].ConnectionVM.Dest.NetworkItemVM;
            }
            // 递归算出左右子树的字符串，要检查是否需要加括号
            string leftStr = recursive_eval(leftNode, true);
            string rightStr = recursive_eval(rightNode, true);
            // 对于AU和EU要特殊处理
            if (str == "EU")
            {
                return "E[" + leftStr + "U" + rightStr + "]";
            }
            else if (str == "AU")
            {
                return "A[" + leftStr + "U" + rightStr + "]";
            }
            return leftStr + str + rightStr;
        }

        // 给定关系字符串str，在一元关系结点上计算出"str(子)"
        public static string eval_one(string str, NetworkItem_VM node)
        {
            NetworkItem_VM childNode;
            if (node.ConnectorVMs[2].ConnectionVM == null)
            {
                childNode = null;
            }
            else if (node.ConnectorVMs[2].ConnectionVM.Dest == node.ConnectorVMs[2])
            {
                childNode = node.ConnectorVMs[2].ConnectionVM.Source.NetworkItemVM;
            }
            else
            {
                childNode = node.ConnectorVMs[2].ConnectionVM.Dest.NetworkItemVM;
            }
            return str + "(" + recursive_eval(childNode) + ")";
        }


        #endregion
    }
}
