using sbid._M;
using sbid._V;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class Axiom_VM : NetworkItem_VM
    {
        private Axiom axiom;

        public Axiom_VM()
        {
            axiom = new Axiom();
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
            // 将所有的Process也传入,作为ProcessMethod的可选进程
            foreach (ViewModelBase item in ResourceManager.mainWindowVM.SelectedItem.SelectedItem.SelectedItem.UserControlVMs)
            {
                if (item is Process_VM)
                {
                    ((Axiom_EW_VM)axiomEWV.DataContext).Processes.Add(((Process_VM)item).Process);
                }
            }

            axiomEWV.ShowDialog(ResourceManager.mainWindowV);
            ResourceManager.mainWindowVM.Tips = "打开了公理：" + axiom.Name + "的编辑窗体";
        }

        #endregion
    }
}
