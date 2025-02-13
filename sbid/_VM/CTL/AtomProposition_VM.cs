﻿using sbid._M;
using sbid._V;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    // 原子命题ViewModel
    public class AtomProposition_VM : NetworkItem_VM
    {
        private AtomProposition atomProposition;

        public AtomProposition_VM()
        {
            atomProposition = new AtomProposition();

            X = 100;
            Y = 100;

            // 左上角锚点中心位置
            double baseX = X + 6;
            double baseY = Y + 8;
            // 横纵方向锚点间距
            double deltaX = 24.5;
            double deltaY = 16;

            init_connector(baseX, baseY, deltaX, deltaY);
        }

        public AtomProposition_VM(double x, double y)
        {
            atomProposition = new AtomProposition();

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

        public AtomProposition AtomProposition { get => atomProposition; }

        #region 右键菜单命令

        // 打开编辑窗口
        public void EditAtomPropositionVM()
        {
            AtomProposition_EW_V apEWV = new AtomProposition_EW_V()
            {
                DataContext = new AtomProposition_EW_VM()
                {
                    AtomProposition = atomProposition
                }
            };

            // 将所有的Process也传入，作为原子命题编辑窗口中属性导航器的可选进程
            foreach (ViewModelBase item in ResourceManager.mainWindowVM.SelectedItem.PanelVMs[0].SidePanelVMs[0].UserControlVMs)
            {
                if (item is Process_VM)
                {
                    Process_VM processVM = (Process_VM)item;
                    ((AtomProposition_EW_VM)apEWV.DataContext).Processes.Add(processVM.Process);
                }
            }

            apEWV.ShowDialog(ResourceManager.mainWindowV);
            ResourceManager.mainWindowVM.Tips = "打开了原子命题的编辑窗体";
        }

        // 计算CTL公式
        public void CalculateCTLFormula()
        {
            CTLTree_P_VM ctlTree_P_VM = (CTLTree_P_VM)ResourceManager.mainWindowVM.SelectedItem.SelectedItem.SelectedItem;
            ctlTree_P_VM.CTLFormula.Content = CTLTree_P_VM.recursive_eval(this);
        }

        // 删除原子命题结点
        private void DeleteAtomPropositionVM()
        {
            CTLTree_P_VM cTLTree_P_VM = (CTLTree_P_VM)ResourceManager.mainWindowVM.SelectedItem.PanelVMs[4].SelectedItem;
            if(cTLTree_P_VM.ActiveConnector != null)
            {
                cTLTree_P_VM.ActiveConnector.IsActive = false;
                cTLTree_P_VM.ActiveConnector = null;
            }
            Utils.deleteAndClearNetworkItemVM(this, cTLTree_P_VM);
            ResourceManager.mainWindowVM.Tips = "删除了原子命题节点：" + atomProposition.RefName.Content;
        }

        #endregion

        #region 私有

        // 补充构造：初始化所有的锚点
        private void init_connector(double baseX, double baseY, double deltaX, double deltaY)
        {
            ConnectorVMs = new ObservableCollection<Connector_VM>();

            // 原子命题具有唯一锚点
            ConnectorVMs.Add(new Connector_VM(baseX + 2 * deltaX, baseY + 0 * deltaY));

            // 将这些锚点所在的NetworkItem_VM回引写入
            foreach (Connector_VM connector_VM in ConnectorVMs)
            {
                connector_VM.NetworkItemVM = this;
            }
        }

        #endregion
    }
}
