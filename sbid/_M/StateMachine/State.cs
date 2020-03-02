﻿using ReactiveUI;
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
            this.name = "未命名" + this.id;
        }

        public string Name { get => name; set => this.RaiseAndSetIfChanged(ref name, value); }
        public int Id { get => id; set => id = value; }
    }
}
