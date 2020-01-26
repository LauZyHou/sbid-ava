using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ReactiveUI;
using sbid._M;
using sbid.ExtraControls;

namespace sbid._VM
{
    public class MainWindow_VM : ReactiveObject
    {
        #region 字段

        private string tips = "123";
        private ObservableCollection<Protocol_VM> protocolVMs = new ObservableCollection<Protocol_VM>();

        #endregion

        #region 命令控制

        // 添加协议
        public void AddProtocol()
        {
            protocolVMs.Add(new Protocol_VM());
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

        #endregion
    }
}
