using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // 实例类，模拟Type(包括UserType)创造的对象实例
    // 构造拓扑图时，需要将进程模板的Attribute例化到这个类型的对象上来
    public abstract class Instance : ReactiveObject
    {
        private Type type;
        private string identifier;
        private bool isArray;

        protected Instance(Attribute attribute)
        {
            this.type = attribute.Type;
            this.identifier = attribute.Identifier;
            this.isArray = attribute.IsArray;
        }

        // 实例的类型，注意对ReferenceInstance只能传UserType
        public Type Type { get => type; }
        // 实例的变量名
        public string Identifier { get => identifier; }
        // 是否是数组
        public bool IsArray { get => isArray; }

        public override string ToString()
        {
            return type.Name + (isArray ? "[]" : "") + " " + identifier + " ";
        }
    }
}
