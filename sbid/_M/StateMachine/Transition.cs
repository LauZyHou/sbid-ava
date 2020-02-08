using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._M
{
    public class Transition
    {
        private string guard = "True";
        private ObservableCollection<string> actions = new ObservableCollection<string>();

        public Transition()
        {
            test_data();
        }

        public string Guard { get => guard; set => guard = value; }
        public ObservableCollection<string> Actions { get => actions; set => actions = value; }

        private void test_data()
        {
            actions.Add("测试Action");
            actions.Add("测试Action");
        }
    }
}
