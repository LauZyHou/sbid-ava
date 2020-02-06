using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    // 可拖拽/移动/锚点连线的面板VM基类
    public class Network_P_VM : ViewModelBase
    {
        private ObservableCollection<NetworkItem_VM> networkItemVMs = new ObservableCollection<NetworkItem_VM>();
        private ObservableCollection<ViewModelBase> userControlVMs = new ObservableCollection<ViewModelBase>();

        // 网络中的结点,每个结点都是NetworkItem_VM
        public ObservableCollection<NetworkItem_VM> NetworkItemVMs { get => networkItemVMs; set => networkItemVMs = value; }
        // 通用结点,用于状态机等需要连线的图(todo 实现成熟后代替掉上面的NetworkItemVMs)
        public ObservableCollection<ViewModelBase> UserControlVMs { get => userControlVMs; set => userControlVMs = value; }
    }
}
