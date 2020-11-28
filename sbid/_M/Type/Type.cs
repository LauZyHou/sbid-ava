using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            this.name = "T" + this.id;
        }

        // 类型名
        public string Name { get => name; set => this.RaiseAndSetIfChanged(ref name, value); }
        // 自增Id
        public int Id
        {
            get => id;
            set
            {
                id = value;
                // 在手动设置Id时(读取项目)要检查当前设置值是否比计数器还要大
                // 如果比计数器大，那么要将计数器至少置为这个值，下次_id++才能比当前id大
                if (value > _id)
                    _id = value;
            }
        }

        #region 计算属性

        // 是否是基本类型
        public bool Basic { get => !(this is UserType); }

        // 是否有继承关系
        public bool Extend { get => this is UserType && ((UserType)this).Parent != null; }

        #endregion

        // 系统内写死的内置类型,使用此唯一引用,且不允许修改
        public static readonly Type TYPE_INT = new Type("int");
        public static readonly Type TYPE_BOOL = new Type("bool");
        public static readonly Type TYPE_NUM = new Type("number");
        public static readonly Type TYPE_BYTE = new Type("byte");
        // 非基本类型，但也是写死的
        public static readonly Type TYPE_BYTE_VEC = new UserType() { Name = "ByteVec" };
        // 在下面静态构造中传入对象
        public static readonly Type TYPE_TIMER;
        public static readonly Type TYPE_MESSAGE;

        static Type() {
            // 构造TYPE_TIMER只读对象
            UserType ut = new UserType() { Name = "Timer" };
            //ut.Attributes.Add(new Attribute(TYPE_NUM, "time"));
            ut.Methods.Add(new Method(TYPE_NUM, "start", new ObservableCollection<Attribute>()));
            ut.Methods.Add(new Method(TYPE_NUM, "timeout", new ObservableCollection<Attribute>()));
            TYPE_TIMER = ut;
            // 构造TYPE_MESSAGE只读对象
            ut = new UserType() { Name = "Message" };
            ut.Parent = (UserType)TYPE_BYTE_VEC;
            TYPE_MESSAGE = ut;
        }
    }
}
