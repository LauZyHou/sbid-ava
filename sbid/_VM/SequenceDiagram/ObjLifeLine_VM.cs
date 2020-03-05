using sbid._M;
using sbid._V;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class ObjLifeLine_VM : NetworkItem_VM
    {
        private static int _id = 0;
        private SeqObject seqObject;

        public ObjLifeLine_VM()
        {
            _id++;
            double baseX = X + 70;
            double baseY = Y + 54;
            double deltaY = 18;

            ConnectorVMs = new ObservableCollection<Connector_VM>();
            for (int i = 0; i < 20; i++)
            {
                ConnectorVMs.Add(new Connector_VM(baseX, baseY + i * deltaY));
            }

            seqObject = new SeqObject("对象名" + _id, "类名" + _id);
        }

        public ObjLifeLine_VM(double x, double y)
        {
            _id++;
            X = x;
            Y = y;

            double baseX = x + 70;
            double baseY = y + 54;
            double deltaY = 18;

            ConnectorVMs = new ObservableCollection<Connector_VM>();
            for (int i = 0; i < 20; i++)
            {
                ConnectorVMs.Add(new Connector_VM(baseX, baseY + i * deltaY));
            }

            seqObject = new SeqObject("对象名" + _id, "类名" + _id);
        }

        // 序列图对象
        public SeqObject SeqObject { get => seqObject; set => seqObject = value; }

        #region 右键菜单命令

        // 尝试打开编辑对象-生命线的窗口
        public void EditObjLifeLine()
        {
            // 从主窗体打开编辑窗体,并在其DataContext中集成当前ObjLifeLine_VM里集成的ObjLifeLine对象,以能对其作修改
            ObjLifeLine_EW_V objLifeLineEWV = new ObjLifeLine_EW_V()
            {
                DataContext = new ObjLifeLine_EW_VM()
                {
                    SeqObject = seqObject
                }
            };
            objLifeLineEWV.ShowDialog(ResourceManager.mainWindowV);
            ResourceManager.mainWindowVM.Tips = "打开了对象-生命线的编辑窗体";
        }

        #endregion
    }
}
