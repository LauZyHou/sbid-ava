using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._M
{
    public class Transition : ReactiveObject
    {
        private string guard = "True";
        private ObservableCollection<Formula> actions = new ObservableCollection<Formula>();

        public Transition()
        {
            //test_data();
        }

        public string Guard { get => guard; set => this.RaiseAndSetIfChanged(ref guard, value); }
        public ObservableCollection<Formula> Actions { get => actions; set => actions = value; }

        private void test_data()
        {
            actions.Add(new Formula("测试Action 1"));
            actions.Add(new Formula("测试Action 2"));
        }
    }
}
