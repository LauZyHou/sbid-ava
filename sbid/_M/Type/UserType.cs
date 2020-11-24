using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._M
{
    // 用户自定义类型,继承普适类型类,多加了一个参数列表
    public class UserType : Type
    {
        private ObservableCollection<Attribute> attributes = new ObservableCollection<Attribute>();
        private ObservableCollection<Method> methods = new ObservableCollection<Method>();
        private UserType parent = null;

        public UserType() : base()
        {
            //test_data();
        }

        // 仅传入一个Attribute的构造，目前只是给Timer用的构造
        public UserType(Type type, string identifier) : base()
        {
            attributes.Add(new Attribute(type, identifier));
        }

        public ObservableCollection<Attribute> Attributes { get => attributes; set => attributes = value; }
        public ObservableCollection<Method> Methods { get => methods; set => methods = value; }
        public UserType Parent
        {
            get => parent;
            set
            {
                this.RaiseAndSetIfChanged(ref parent, value);
                this.RaisePropertyChanged("Extend"); // ？
                // 继承关系变化时，总是清除字段长度的取值
                foreach (Attribute attribute in attributes)
                {
                    attribute.Len = "";
                }
                // 通过这里通知属性变化，来让前端的字段长度部分显示/不显示
                this.RaisePropertyChanged(nameof(HaveAttrLen));
            }
        }
        // 这个数据类型的各个字段是否有字段长度（仅用于前端是否显示）
        public bool HaveAttrLen
        {
            get {
                // 判断是不是继承ByteVec，如果是，就有字段长度
                return parent == Type.TYPE_BYTE_VEC;
            }
        }

        private void test_data()
        {
            attributes.Add(new Attribute(Type.TYPE_INT, "a"));
            attributes.Add(new Attribute(Type.TYPE_BOOL, "b"));
        }
    }
}
