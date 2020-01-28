using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    // 各个子面板下的侧栏面板的超类,各个具体的侧栏面板要继承此类
    public abstract class SidePanel_VM : ViewModelBase
    {
        private string name;

        public string Name
        {
            get => name;
            set => this.RaiseAndSetIfChanged(ref name, value);
        }
    }
}
