﻿using sbid._M;
using sbid._V;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    // 拓扑图结点VM
    public class TopoNode_VM : NetworkItem_VM
    {
        private TopoNode topoNode = new TopoNode();

        public TopoNode_VM()
        {
            init();
        }

        public TopoNode_VM(double x, double y)
        {
            X = x;
            Y = y;
            init();
        }

        public TopoNode TopoNode { get => topoNode; }

        #region 右键菜单命令

        // 尝试打开编辑当前拓扑结点的窗体
        public void EditTopoNodeVM()
        {
            // 从主窗体打开编辑窗体,并在其DataContext中集成当前TopoNode_VM里集成的TopoNode对象,以能对其作修改
            TopoNode_EW_V topoNodeEWV = new TopoNode_EW_V()
            {
                DataContext = new TopoNode_EW_VM()
                {
                    TopoNode = topoNode
                }
            };
            topoNodeEWV.ShowDialog(ResourceManager.mainWindowV);
            ResourceManager.mainWindowVM.Tips = "打开了拓扑结点：" + topoNode.Name + "的编辑窗体";
        }

        #endregion

        #region 私有

        // 辅助构造
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
            ConnectorVMs.Add(new Connector_VM(baseX + 0,
                                              baseY + r));
            ConnectorVMs.Add(new Connector_VM(baseX + d,
                                              baseY + d));
            ConnectorVMs.Add(new Connector_VM(baseX + r,
                                              baseY + 0));
            ConnectorVMs.Add(new Connector_VM(baseX + 2 * r - d,
                                              baseY + d));
            ConnectorVMs.Add(new Connector_VM(baseX + 2 * r,
                                              baseY + r));
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
    }
}
