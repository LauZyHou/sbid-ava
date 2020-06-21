using Avalonia;
using sbid._M;
using sbid._V;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class StateTrans_VM : NetworkItem_VM
    {
        private StateTrans stateTrans = new StateTrans();

        public StateTrans_VM() { }

        // 构造时添加8个锚点
        public StateTrans_VM(double x, double y)
        {
            X = x;
            Y = y;
            ConnectorVMs = new ObservableCollection<Connector_VM>();

            // 左上角锚点中心位置
            double baseX = X + 4;
            double baseY = Y + 4;

            // 8个锚点，只需要设置好第一个，其它锚点可以后续通过它的位置配合H/W刷新确定
            ConnectorVMs.Add(new Connector_VM(baseX, baseY));
            for (int i = 0; i < 7; i++)
            {
                ConnectorVMs.Add(new Connector_VM());
            }
        }

        public StateTrans StateTrans { get => stateTrans; }

        #region 右键菜单命令

        // 打开编辑窗口
        public void EditStateTrans()
        {
            StateTrans_EW_V stateTransEWV = new StateTrans_EW_V()
            {
                DataContext = new StateTrans_EW_VM()
                {
                    StateTrans = stateTrans
                }
            };
            stateTransEWV.ShowDialog(ResourceManager.mainWindowV);
            ResourceManager.mainWindowVM.Tips = "打开了转移关系的编辑窗体";
        }

        #endregion

        // 刷新锚点位置
        public new void FlushConnectorPos()
        {
            // 左上角锚点位置
            double x0 = ConnectorVMs[0].Pos.X;
            double y0 = ConnectorVMs[0].Pos.Y;
            // 一个一个确定
            Point pos = new Point(x0 + W / 2, y0);
            ConnectorVMs[1].Pos = pos;

            pos = new Point(x0 + W, y0);
            ConnectorVMs[2].Pos = pos;

            pos = new Point(x0, y0 + H / 2);
            ConnectorVMs[3].Pos = pos;

            pos = new Point(x0 + W, y0 + H / 2);
            ConnectorVMs[4].Pos = pos;

            pos = new Point(x0, y0 + H);
            ConnectorVMs[5].Pos = pos;

            pos = new Point(x0 + W / 2, y0 + H);
            ConnectorVMs[6].Pos = pos;

            pos = new Point(x0 + W, y0 + H);
            ConnectorVMs[7].Pos = pos;
        }
    }
}
