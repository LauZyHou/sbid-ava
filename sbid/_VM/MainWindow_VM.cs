using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ReactiveUI;
using sbid._M;
using sbid.ExtraControls;

namespace sbid._VM
{
    public class MainWindow_VM : ViewModelBase
    {
        #region 字段

        private string tips = "123";
        private ObservableCollection<Protocol_VM> protocolVMs = new ObservableCollection<Protocol_VM>();
        private Protocol_VM selectedItem;

        #endregion

        #region 命令控制

        // 添加新协议
        public void AddProtocol()
        {
            Protocol_VM protocol_VM = new Protocol_VM();
            protocolVMs.Add(protocol_VM);
            SelectedItem = protocol_VM;
        }

        // 添加攻击树
        public void addAttackTree()
        {

        }

        // 添加CTL公式
        public void addCTL()
        {

        }

        // 添加顺序图(Sequence)
        public void addSequence()
        {

        }

        // 添加拓扑图
        public void addTopoGraph()
        {

        }

        #endregion

        #region 属性

        public string Tips
        {
            get => tips;
            set => this.RaiseAndSetIfChanged(ref tips, value);
        }

        public ObservableCollection<Protocol_VM> ProtocolVMs { get => protocolVMs; set => protocolVMs = value; }
        // 记录当前选中项,用于在打开新面板时立即切换过去
        public Protocol_VM SelectedItem
        {
            get => selectedItem;
            set => this.RaiseAndSetIfChanged(ref selectedItem, value);
        }

        #endregion
    }
}
