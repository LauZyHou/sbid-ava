using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._M
{
    // 状态机转移
    public class StateTrans
    {
        private ObservableCollection<Formula> guards = new ObservableCollection<Formula>();
        private ObservableCollection<Formula> actions = new ObservableCollection<Formula>();

        public ObservableCollection<Formula> Guards { get => guards; }
        public ObservableCollection<Formula> Actions { get => actions; }
    }
}
