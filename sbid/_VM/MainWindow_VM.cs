using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ReactiveUI;
using System.Reactive;
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

        #region 构造

        public MainWindow_VM()
        {
            //// 指示添加类图等命令是否可用的IObservable对象
            //IObservable<bool> addEnabled = this.WhenAnyValue(
            //    x => x.SelectedItem,
            //    x => !Protocol_VM.IsNull(x)
            //    );

            //// 生成命令
            //AddClassDiagram = ReactiveCommand.Create(
            //    () => new ClassDiagram_P_VM(),
            //    addEnabled
            //    );
        }

        #endregion

        #region 命令控制

        // 添加新协议
        public void AddProtocol()
        {
            Protocol_VM protocol_VM = new Protocol_VM();
            protocolVMs.Add(protocol_VM);
            SelectedItem = protocol_VM;
        }

        // 添加新类图
        public void AddClassDiagram()
        {
            // 判定协议已创建
            if (selectedItem == null)
                return;

            // 切换到当前协议的类图面板下
            selectedItem.SelectedItem = selectedItem.PanelVMs[0];

            // 添加过程
            ClassDiagram_P_VM pvm = new ClassDiagram_P_VM();
            selectedItem.PanelVMs[0].SidePanelVMs.Add(pvm);
            selectedItem.PanelVMs[0].SelectedItem = pvm;
        }

        // 添加新拓扑图
        public void AddTopoGraph()
        {
            if (selectedItem == null)
                return;

            selectedItem.SelectedItem = selectedItem.PanelVMs[2];

            TopoGraph_P_VM pvm = new TopoGraph_P_VM();
            selectedItem.PanelVMs[2].SidePanelVMs.Add(pvm);
            selectedItem.PanelVMs[2].SelectedItem = pvm;
        }

        // 添加新攻击树
        public void AddAttackTree()
        {
            if (selectedItem == null)
                return;

            selectedItem.SelectedItem = selectedItem.PanelVMs[3];

            AttackTree_P_VM pvm = new AttackTree_P_VM();
            selectedItem.PanelVMs[3].SidePanelVMs.Add(pvm);
            selectedItem.PanelVMs[3].SelectedItem = pvm;
        }

        // 添加新CTL语法树
        public void AddCTLTree()
        {
            if (selectedItem == null)
                return;

            selectedItem.SelectedItem = selectedItem.PanelVMs[4];

            CTLTree_P_VM pvm = new CTLTree_P_VM();
            selectedItem.PanelVMs[4].SidePanelVMs.Add(pvm);
            selectedItem.PanelVMs[4].SelectedItem = pvm;
        }

        #endregion

        #region 属性

        // 下方提示条内容
        public string Tips
        {
            get => tips;
            set => this.RaiseAndSetIfChanged(ref tips, value);
        }

        // 集成所有的协议
        public ObservableCollection<Protocol_VM> ProtocolVMs { get => protocolVMs; set => protocolVMs = value; }
        
        // 记录当前选中项,用于在打开新面板时立即切换过去
        public Protocol_VM SelectedItem
        {
            get => selectedItem;
            set => this.RaiseAndSetIfChanged(ref selectedItem, value);
        }

        // [命令]添加类图
        //public ReactiveCommand<Unit, ClassDiagram_P_VM> AddClassDiagram { get; set; }

        #endregion
    }
}
