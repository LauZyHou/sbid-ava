using sbid._M;
using sbid._V;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class Axiom_VM : NetworkItem_VM
    {
        private static int _id = 1;
        private Axiom axiom;

        public Axiom_VM()
        {
            axiom = new Axiom("未命名" + _id);
            _id++;
        }

        public Axiom Axiom { get => axiom; set => axiom = value; }

        #region 右键菜单命令

        // 尝试打开当前Axiom_VM的编辑窗体
        public void EditAxiomVM()
        {
            // 从主窗体打开编辑窗体,并在其DataContext中集成当前Axiom_VM里集成的Axiom对象,以能对其作修改
            Axiom_EW_V axiomEWV = new Axiom_EW_V()
            {
                DataContext = new Axiom_EW_VM()
                {
                    Axiom = axiom
                }
            };
            // 将所有的Type也传入,作为Method的可用类型
            foreach (NetworkItem_VM item in ResourceManager.mainWindowVM.SelectedItem.SelectedItem.SelectedItem.NetworkItemVMs)
            {
                if (item is UserType_VM)
                {
                    ((Axiom_EW_VM)axiomEWV.DataContext).Types.Add(((UserType_VM)item).Type);
                }
            }

            axiomEWV.ShowDialog(ResourceManager.mainWindowV);
            ResourceManager.mainWindowVM.Tips = "打开了公理：" + axiom.Name + "的编辑窗体";
        }

        #endregion
    }
}
