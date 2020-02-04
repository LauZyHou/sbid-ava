using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._M
{
    // 通信方法的输入输出枚举
    public enum InOut
    {
        In, Out
    }

    // 进程模板中的通信方法
    public class CommMethod : ReactiveObject
    {
        private string name;
        private ObservableCollection<Attribute> parameters; // 这里不创建,在构造时传入
        private InOut inOutSuffix;

        // 函数名
        public string Name { get => name; set => this.RaiseAndSetIfChanged(ref name, value); }
        // 参数列表
        public ObservableCollection<Attribute> Parameters { get => parameters; set => parameters = value; }
        // 输入输出(必选)
        public InOut InOutSuffix { get => inOutSuffix; set => this.RaiseAndSetIfChanged(ref inOutSuffix, value); }
    }
}
