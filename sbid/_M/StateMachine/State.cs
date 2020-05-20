using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    public class State : ReactiveObject
    {
        public static int _id = 0;
        private string name;
        private int id;

        public State()
        {
            _id++;
            this.id = _id;
            this.name = "S" + this.id;
        }

        public string Name { get => name; set => this.RaiseAndSetIfChanged(ref name, value); }
        public int Id
        {
            get => id;
            set
            {
                id = value;
                if (value > _id)
                    _id = value;
            }
        }

        // 用于最顶层的状态机面板维护的状态
        public static readonly State TopState = new State() { Name="顶层" };
    }
}
