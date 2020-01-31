using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // 普适的类型类,基本类型int,bool也属于此类
    public class Type
    {
        private string name;

        public Type(string name)
        {
            this.name = name;
        }

        // 类型名
        public string Name { get => name; set => name = value; }

        // 系统内写死的两个内置类型,使用此唯一引用,且不允许修改
        public static readonly Type TYPE_INT = new Type("int");
        public static readonly Type TYPE_BOOL = new Type("bool");
    }
}
