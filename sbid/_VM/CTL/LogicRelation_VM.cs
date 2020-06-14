﻿using ReactiveUI;
using sbid._M;
using sbid._V;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class LogicRelation_VM : NetworkItem_VM
    {
        private LogicRelation logicRelation;

        #region 构造和初始化

        public LogicRelation_VM()
        {
            init();
        }

        public LogicRelation_VM(double x, double y)
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

        public LogicRelation LogicRelation { get => logicRelation; set => this.RaiseAndSetIfChanged(ref logicRelation, value); }

        #region 右键菜单命令

        // 打开编辑窗口
        public void EditLogicRelationVM()
        {
            LogicRelation_EW_V ctlRelationEWV = new LogicRelation_EW_V()
            {
                DataContext = new LogicRelation_EW_VM()
                {
                    LogicRelation_VM = this
                }
            };
            ctlRelationEWV.ShowDialog(ResourceManager.mainWindowV);
            ResourceManager.mainWindowVM.Tips = "打开了Logic关系结点的编辑窗体";
        }

        // 计算CTL公式
        public void CalculateCTLFormula()
        {
            CTLTree_P_VM ctlTree_P_VM = (CTLTree_P_VM)ResourceManager.mainWindowVM.SelectedItem.SelectedItem.SelectedItem;
            ctlTree_P_VM.CTLFormula.Content = CTLTree_P_VM.recursive_eval(this);
        }

        #endregion
    }
}