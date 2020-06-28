using Avalonia;
using ReactiveUI;
using sbid._M;
using sbid._V;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class TopoEdge_VM : Connection_VM
    {
        private TopoEdge topoEdge = new TopoEdge();

        public TopoEdge TopoEdge { get => topoEdge; set => this.RaiseAndSetIfChanged(ref topoEdge, value); }

        // 两锚点中心位置附近的点
        public Point MidPos
        {
            get
            {
                double x = (Source.Pos.X + Dest.Pos.X) / 2;
                double y = (Source.Pos.Y + Dest.Pos.Y) / 2;
                return new Point(x - 20, y - 10);
            }
        }

        #region 右键菜单命令

        // 尝试打开编辑拓扑图边的窗口
        public void EditTopoEdgeVM()
        {
            // 计算可选的所有CommMethodPair
            List<CommMethodPair> commMethodPairs = new List<CommMethodPair>();
            // 首先获取两端的Process，Source方是processA，Dest方是processB
            Process processA = ((TopoNode_VM)Source.NetworkItemVM).TopoNode.Process;
            Process processB = ((TopoNode_VM)Dest.NetworkItemVM).TopoNode.Process;
            // 搜索所有的CommChannel_VM
            foreach (ViewModelBase item in ResourceManager.mainWindowVM.SelectedItem.PanelVMs[0].SidePanelVMs[0].UserControlVMs)
            {
                if (item is CommChannel_VM)
                {
                    CommChannel_VM commChannel_VM = (CommChannel_VM)item;
                    // 在其中搜索所有的CommMethodPair
                    foreach (CommMethodPair commMethodPair in commChannel_VM.CommChannel.CommMethodPairs)
                    {
                        // 如果是A->B，就把它加进来
                        if (commMethodPair.ProcessA == processA && commMethodPair.ProcessB == processB)
                        {
                            commMethodPairs.Add(commMethodPair);
                        }
                    }
                }
            }
            // 从主窗体打开编辑窗体,并在其DataContext中集成当前TopoEdge_VM里集成的TopoEdge对象,以能对其作修改
            TopoEdge_EW_V topoEdgeEWV = new TopoEdge_EW_V()
            {
                DataContext = new TopoEdge_EW_VM()
                {
                    TopoEdge = topoEdge,
                    CommMethodPairs = commMethodPairs
                }
            };
            topoEdgeEWV.ShowDialog(ResourceManager.mainWindowV);
            ResourceManager.mainWindowVM.Tips = "打开了拓扑图边的编辑窗体";
        }

        #endregion
    }
}
