using sbid._M;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    // 状态机转移结点的编辑
    public class StateTrans_EW_VM
    {
        private StateTrans stateTrans;
        private ObservableCollection<Type> types = new ObservableCollection<Type>();
        private Process process;
        private ObservableCollection<Nav> properties = new ObservableCollection<Nav>();

        // 要编辑的状态转移
        public StateTrans StateTrans { get => stateTrans; set => stateTrans = value; }
        // 类图中所有已经存在的类型       
        public ObservableCollection<Type> Types { get => types; }
        // 所在的进程模板
        public Process Process { get => process; set => process = value; }
        // 属性选择器例化的Nav
        public ObservableCollection<Nav> Properties { get => properties; }
    }
}
