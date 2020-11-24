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
        private bool isArray;
        private int id;
        private string len = "";

        public Attribute(Type type, string identifier, bool isArray = false, bool isTemp = false)
        {
            this.type = type;
            this.identifier = identifier;
            this.isArray = isArray;
            // 一般在创建时都不是临时的Attribute
            // 但有的时候只是需要创建一个临时的，而不可能保存他
            // 临时的Attribute不会占用新的_id号
            if (!isTemp)
            {
                _id++;
                this.id = _id;
            }
        }

        public Attribute(Attribute attribute)
        {
            this.type = attribute.type;
            this.identifier = attribute.identifier;
            this.isArray = attribute.isArray;
            _id++;
            this.id = _id; // 注意拷贝构造时也使用新id
        }

        public Type Type { get => type; set => this.RaiseAndSetIfChanged(ref type, value); }
        public string Identifier { get => identifier; set => this.RaiseAndSetIfChanged(ref identifier, value); }
        public bool IsArray { get => isArray; set => this.RaiseAndSetIfChanged(ref isArray, value); }
        public string Len { get => len; set => this.RaiseAndSetIfChanged(ref len, value); }
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

        public override string ToString()
        {
            return type.Name + (IsArray ? "[]" : "") + " " + identifier;
        }
    }
}
