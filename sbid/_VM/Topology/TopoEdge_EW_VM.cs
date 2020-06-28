using sbid._M;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class TopoEdge_EW_VM
    {
        private TopoEdge topoEdge;
        private List<CommMethodPair> commMethodPairs;

        // 要编辑的拓扑图边
        public TopoEdge TopoEdge { get => topoEdge; set => topoEdge = value; }
        // 所有可选的CommMethodPair，在打开窗体前通过连线双方的拓扑结点Process来计算并写入
        public List<CommMethodPair> CommMethodPairs { get => commMethodPairs; set => commMethodPairs = value; }
    }
}
