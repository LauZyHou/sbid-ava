using Avalonia.Controls;
using sbid._M;
using sbid._V;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    // 对象-生命线的VM
    public class ObjLifeLine_VM : NetworkItem_VM
    {
        private SeqObject seqObject = new SeqObject(null);

        #region 构造和初始化

        public ObjLifeLine_VM()
        {
            init_connector();
        }

        public ObjLifeLine_VM(double x, double y)
        {
            X = x;
            Y = y;

            init_connector();
        }

        private void init_connector()
        {
            double baseX = X + 70;
            double baseY = Y + 54;
            double deltaY = 18;

            ConnectorVMs = new ObservableCollection<Connector_VM>();
            for (int i = 0; i < 20; i++)
            {
                ConnectorVMs.Add(new Connector_VM(baseX, baseY + i * deltaY));
            }

            // 将这些锚点所在的NetworkItem_VM回引写入
            foreach (Connector_VM connector_VM in ConnectorVMs)
            {
                connector_VM.NetworkItemVM = this;
            }
        }

        #endregion

        // 序列图对象
        public SeqObject SeqObject { get => seqObject; }

        #region 右键菜单命令

        // 尝试打开编辑对象-生命线的窗口
        public void EditObjLifeLine()
        {
            // 从主窗体打开编辑窗体,并在其DataContext中集成当前ObjLifeLine_VM里集成的ObjLifeLine对象,以能对其作修改
            ObjLifeLine_EW_V objLifeLineEWV = new ObjLifeLine_EW_V()
            {
                DataContext = new ObjLifeLine_EW_VM()
                {
                    SeqObject = seqObject,
                    ObjLifeLine_VM = this
                }
            };
            // 将所有的Process传入,作为SeqObject去选用的参数
            foreach (ViewModelBase item in ResourceManager.mainWindowVM.SelectedItem.PanelVMs[0].SidePanelVMs[0].UserControlVMs)
            {
                if (item is Process_VM)
                {
                    ((ObjLifeLine_EW_VM)objLifeLineEWV.DataContext).Processes.Add(((Process_VM)item).Process);
                }
            }

            // [bugfix]这里加锁保护一下，防止触发process_ComboBox_Changed方法导致身上的消息连线被删除
            ((ObjLifeLine_EW_VM)objLifeLineEWV.DataContext).SafetyLock = true;
            // [bugfix]因为在xaml里绑定Process打开编辑窗口显示出不来，只好在这里手动设置一下
            ComboBox process_ComboBox = ControlExtensions.FindControl<ComboBox>(objLifeLineEWV, "process_ComboBox");
            process_ComboBox.SelectedItem = ((ObjLifeLine_EW_VM)objLifeLineEWV.DataContext).SeqObject.Process;
            // 设置完后把锁解除
            ((ObjLifeLine_EW_VM)objLifeLineEWV.DataContext).SafetyLock = false;

            objLifeLineEWV.ShowDialog(ResourceManager.mainWindowV);
            ResourceManager.mainWindowVM.Tips = "打开了对象-生命线的编辑窗体";

            // 【bugfix】打开后，禁用当前窗体
            SequenceDiagram_P_VM seqDiagramP_VM = (SequenceDiagram_P_VM)ResourceManager.mainWindowVM.SelectedItem.SelectedItem.SelectedItem;
            seqDiagramP_VM.PanelEnabled = false;
        }

        // 删除当前的对象-生命线
        private void DeleteObjLifeLine()
        {
            SequenceDiagram_P_VM sequenceDiagram_P_VM = (SequenceDiagram_P_VM)ResourceManager.mainWindowVM.SelectedItem.PanelVMs[5].SelectedItem;
            if (sequenceDiagram_P_VM.ActiveConnector != null)
            {
                sequenceDiagram_P_VM.ActiveConnector.IsActive = false;
                sequenceDiagram_P_VM.ActiveConnector = null;
            }
            Utils.deleteAndClearNetworkItemVM(this, sequenceDiagram_P_VM);
            ResourceManager.mainWindowVM.Tips = "删除了对象-生命线";
        }

        #endregion
    }
}
