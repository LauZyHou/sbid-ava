using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // 转移关系上执行的动作
    // 因为在Transition中是放在列表里,所以这里即使只有一个公式也单独封装起来,以保证数据绑定时绑定引用
    public class Action : ReactiveObject
    {
        private string formula;

        public Action(string formula)
        {
            this.formula = formula;
        }

        public string Formula { get => formula; set => this.RaiseAndSetIfChanged(ref formula, value); }
    }
}
