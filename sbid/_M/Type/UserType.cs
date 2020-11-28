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
        // 【仅Message】
        private string msgType = "";
        private string signLen = "";

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
                // 通过这里通知属性变化，来让前端的“字段长度”部分显示/不显示
                this.RaisePropertyChanged(nameof(IsExtendByteVec));
                // 通过这里通知属性变化，来让前端的“报文类型”和“签名字段长度”部分显示/不显示
                this.RaisePropertyChanged(nameof(IsExtendMessage));
                // “继承自ByteVec”被破坏时，清除“字段长度”的取值
                if (IsExtendByteVec == false)
                {
                    foreach (Attribute attribute in attributes)
                    {
                        attribute.Len = "";
                    }
                }
                // “继承自Message”被破坏时，清除“报文类型”和“签名字段长度”的取值
                // [bugfix]能够防止读取文件时ByteVec变化导致写入的MsgType和SignLen被清除
                if (IsExtendMessage == false)
                {
                    MsgType = "";
                    SignLen = "";
                }
            }
        }

        // 【仅Message】
        // 报文类型，仅当（直接/间接）继承自Message内置类型时才有
        public string MsgType { get => msgType; set => this.RaiseAndSetIfChanged(ref msgType, value); }
        // 签名字段长度，仅当（直接/间接）继承自Message内置类型时才有
        public string SignLen { get => signLen; set => this.RaiseAndSetIfChanged(ref signLen, value); }

        // 是否继承自Message
        // 如果是，前端就有“报文类型”和“签名字段长度”
        public bool IsExtendMessage
        {
            get
            {
                UserType p = parent;
                while (p != null && p != TYPE_MESSAGE)
                {
                    p = p.parent;
                }
                return p == TYPE_MESSAGE;
            }
        }

        // 是否继承自ByteVec
        // 如果是，前端就有“字段长度”
        public bool IsExtendByteVec
        {
            get
            {
                UserType p = parent;
                while (p != null && p != TYPE_BYTE_VEC)
                {
                    p = p.parent;
                }
                return p == TYPE_BYTE_VEC;
            }
        }

        private void test_data()
        {
            attributes.Add(new Attribute(Type.TYPE_INT, "a"));
            attributes.Add(new Attribute(Type.TYPE_BOOL, "b"));
        }
    }
}
