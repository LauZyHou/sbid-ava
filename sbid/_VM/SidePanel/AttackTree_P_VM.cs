using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class AttackTree_P_VM : SidePanel_VM
    {
        private static int _id = 1;

        // 默认构造时使用默认名称
        public AttackTree_P_VM()
        {
            this.Name = "攻击树" + _id;
            _id++;
        }

        #region 按钮和右键菜单命令

        // 创建攻击结点
        public void CreateAttackVM()
        {
            Attack_VM attackVM = new Attack_VM(0, 0);
            UserControlVMs.Add(attackVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的攻击结点：" + attackVM.Attack.Content;
        }

        // 创建[或]关系
        public void CreateRelationVM_OR()
        {

        }

        // 创建[与]关系
        public void CreateRelationVM_AND()
        {

        }

        // 创建[非]关系
        public void CreateRelationVM_NEG()
        {

        }

        #endregion
    }
}
