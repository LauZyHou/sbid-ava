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
        private ObservableCollection<Action> actions = new ObservableCollection<Action>();

        public Transition()
        {
            test_data();
        }

        public string Guard { get => guard; set => this.RaiseAndSetIfChanged(ref guard, value); }
        public ObservableCollection<Action> Actions { get => actions; set => actions = value; }

        private void test_data()
        {
            actions.Add(new Action("测试Action 1"));
            actions.Add(new Action("测试Action 2"));
        }
    }
}
