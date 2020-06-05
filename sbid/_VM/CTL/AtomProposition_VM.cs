using sbid._M;
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
            apEWV.ShowDialog(ResourceManager.mainWindowV);
            ResourceManager.mainWindowVM.Tips = "打开了原子命题的编辑窗体";
        }

        #endregion

        #region 私有

        // 补充构造：初始化所有的锚点
        private void init_connector(double baseX, double baseY, double deltaX, double deltaY)
        {
            ConnectorVMs = new ObservableCollection<Connector_VM>();

            // 14个锚点,从左上角锚点中心位置进行位置推算
            ConnectorVMs.Add(new Connector_VM(baseX + 0 * deltaX, baseY + 0 * deltaY));
            ConnectorVMs.Add(new Connector_VM(baseX + 1 * deltaX, baseY + 0 * deltaY));
            ConnectorVMs.Add(new Connector_VM(baseX + 2 * deltaX, baseY + 0 * deltaY));
            ConnectorVMs.Add(new Connector_VM(baseX + 3 * deltaX, baseY + 0 * deltaY));
            ConnectorVMs.Add(new Connector_VM(baseX + 4 * deltaX, baseY + 0 * deltaY));

            ConnectorVMs.Add(new Connector_VM(baseX + 0 * deltaX, baseY + 1 * deltaY));
            ConnectorVMs.Add(new Connector_VM(baseX + 4 * deltaX, baseY + 1 * deltaY));

            ConnectorVMs.Add(new Connector_VM(baseX + 0 * deltaX, baseY + 2 * deltaY));
            ConnectorVMs.Add(new Connector_VM(baseX + 4 * deltaX, baseY + 2 * deltaY));

            ConnectorVMs.Add(new Connector_VM(baseX + 0 * deltaX, baseY + 3 * deltaY));
            ConnectorVMs.Add(new Connector_VM(baseX + 1 * deltaX, baseY + 3 * deltaY));
            ConnectorVMs.Add(new Connector_VM(baseX + 2 * deltaX, baseY + 3 * deltaY));
            ConnectorVMs.Add(new Connector_VM(baseX + 3 * deltaX, baseY + 3 * deltaY));
            ConnectorVMs.Add(new Connector_VM(baseX + 4 * deltaX, baseY + 3 * deltaY));

            // 将这些锚点所在的NetworkItem_VM回引写入
            foreach (Connector_VM connector_VM in ConnectorVMs)
            {
                connector_VM.NetworkItemVM = this;
            }
        }

        #endregion
    }
}
