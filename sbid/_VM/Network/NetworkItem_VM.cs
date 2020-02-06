﻿using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    // 可以 移动/选中 的结点
    public class NetworkItem_VM : ViewModelBase
    {
        private double x;
        private double y;
        private ObservableCollection<Connector_VM> connectorVMs; // 锚点表按需创建

        public double X { get => x; set => this.RaiseAndSetIfChanged(ref x, value); }
        public double Y { get => y; set => this.RaiseAndSetIfChanged(ref y, value); }
        public ObservableCollection<Connector_VM> ConnectorVMs { get => connectorVMs; set => connectorVMs = value; }
    }
}
