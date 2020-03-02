using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // 类型->名称 的参数类,如<int,a>,<bool,b>,<Msg,m>
    public class Attribute : ReactiveObject
    {
        public static int _id = 0;
        private Type type;
        private string identifier;
        private int id;

        public Attribute(Type type, string identifier)
        {
            this.type = type;
            this.identifier = identifier;
            _id++;
            this.id = _id;
        }

        public Type Type { get => type; set => this.RaiseAndSetIfChanged(ref type, value); }
        public string Identifier { get => identifier; set => this.RaiseAndSetIfChanged(ref identifier, value); }
        public int Id { get => id; set => id = value; }

        public override string ToString()
        {
            return type.Name + " " + identifier;
        }
    }
}
