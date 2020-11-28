using ReactiveUI;
using sbid._M;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class Process_EW_VM : ViewModelBase
    {
        private Process process;
        private ObservableCollection<_M.Type> types = new ObservableCollection<_M.Type>();

        private ObservableCollection<Attribute> zDParams = new ObservableCollection<Attribute>();
        private ObservableCollection<Attribute> commParams = new ObservableCollection<Attribute>();

        // 要编辑的Process
        public Process Process { get => process; set => process = value; }
        // 所有Type对象(包括int, bool),作为Attribute/Method/CommMethod的可用类型
        public ObservableCollection<sbid._M.Type> Types { get => types; set => types = value; }

        // 【仅窗体用】
        // 是否选择了原生以太网帧
        private bool isNativeEthernetFrame = true;
        public bool IsNativeEthernetFrame { get => isNativeEthernetFrame; set => this.RaiseAndSetIfChanged(ref isNativeEthernetFrame, value); }

        // 自定Method参数列表绑定此处
        public ObservableCollection<Attribute> ZDParams { get => zDParams; set => this.RaiseAndSetIfChanged(ref zDParams, value); }
        // CommMethod参数列表绑定此处
        public ObservableCollection<Attribute> CommParams { get => commParams; set => this.RaiseAndSetIfChanged(ref commParams, value); }
    }
}
