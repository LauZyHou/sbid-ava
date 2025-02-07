﻿using ReactiveUI;
using sbid._M;
using sbid._V;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class CTLRelation_VM : NetworkItem_VM
    {
        private CTLRelation ctlRelation;

        #region 构造和初始化

        public CTLRelation_VM()
        {
            init();
        }

        public CTLRelation_VM(double x, double y)
        {
            X = x;
            Y = y;
            init();
        }

        private void init()
        {
            // 图形左上角点位置(圆的外接正方形左上角点)
            // 左上角锚点中心位置
            double baseX = X + 6;
            double baseY = Y + 6;

            // 圆弧上锚点之间的短delta,短delta+长delta=半径长r
            double d = 2.8 * 3;
            double r = 27;

            ConnectorVMs = new ObservableCollection<Connector_VM>();

            // 顺时针一圈锚点
            ConnectorVMs.Add(new Connector_VM(baseX + r,
                                              baseY + 0));
            ConnectorVMs.Add(new Connector_VM(baseX + 2 * r - d,
                                              baseY + 2 * r - d));
            ConnectorVMs.Add(new Connector_VM(baseX + r,
                                              baseY + 2 * r));
            ConnectorVMs.Add(new Connector_VM(baseX + d,
                                              baseY + 2 * r - d));

            // 将这些锚点所在的NetworkItem_VM回引写入
            foreach (Connector_VM connector_VM in ConnectorVMs)
            {
                connector_VM.NetworkItemVM = this;
            }
        }

        #endregion

        public CTLRelation CTLRelation { get => ctlRelation; set => this.RaiseAndSetIfChanged(ref ctlRelation, value); }

        #region 右键菜单命令

        // 打开编辑窗口
        public void EditCTLRelationVM()
        {
            CTLRelation_EW_V ctlRelationEWV = new CTLRelation_EW_V()
            {
                DataContext = new CTLRelation_EW_VM()
                {
                    CTLRelation_VM = this
                }
            };
            ctlRelationEWV.ShowDialog(ResourceManager.mainWindowV);
            ResourceManager.mainWindowVM.Tips = "打开了CTL关系结点的编辑窗体";

            // 【bugfix】打开后，禁用当前窗体
            CTLTree_P_VM ctlTree_P_VM = (CTLTree_P_VM)ResourceManager.mainWindowVM.SelectedItem.SelectedItem.SelectedItem;
            ctlTree_P_VM.PanelEnabled = false;
        }

        // 计算CTL公式
        public void CalculateCTLFormula()
        {
            CTLTree_P_VM ctlTree_P_VM = (CTLTree_P_VM)ResourceManager.mainWindowVM.SelectedItem.SelectedItem.SelectedItem;
            ctlTree_P_VM.CTLFormula.Content = CTLTree_P_VM.recursive_eval(this);
        }

        // 删除当前CTL关系结点
        private void DeleteCTLRelationVM()
        {
            CTLTree_P_VM cTLTree_P_VM = (CTLTree_P_VM)ResourceManager.mainWindowVM.SelectedItem.PanelVMs[4].SelectedItem;
            if (cTLTree_P_VM.ActiveConnector != null)
            {
                cTLTree_P_VM.ActiveConnector.IsActive = false;
                cTLTree_P_VM.ActiveConnector = null;
            }
            Utils.deleteAndClearNetworkItemVM(this, cTLTree_P_VM);
            ResourceManager.mainWindowVM.Tips = "删除了CTL关系节点";
        }

        #endregion

    }
}
