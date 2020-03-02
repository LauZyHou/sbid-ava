using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // 普适的类型类,基本类型int,bool也属于此类
    public class Type : ReactiveObject
    {
        public static int _id = 0;
        private string name;
        private int id;

        public Type(string name)
        {
            _id++;
            this.id = _id;
            this.name = name;
        }

        public Type()
        {
            _id++;
            this.id = _id;
            this.name = "未命名" + this.id;
        }

        // 类型名
        public string Name { get => name; set => this.RaiseAndSetIfChanged(ref name, value); }
        // 自增Id
        public int Id { get => id; set => id = value; }

        // 系统内写死的两个内置类型,使用此唯一引用,且不允许修改
        public static readonly Type TYPE_INT = new Type("int");
        public static readonly Type TYPE_BOOL = new Type("bool");

    }
}
