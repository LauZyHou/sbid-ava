﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._M
{
    // 用户自定义类型,继承普适类型类,多加了一个参数列表
    public class UserType : Type
    {
        private ObservableCollection<Attribute> attributes = new ObservableCollection<Attribute>();

        public UserType() : base()
        {
            //test_data();
        }

        // 仅传入一个Attribute的构造，目前只是给Timer用的构造
        public UserType(Type type,string identifier) : base()
        {
            attributes.Add(new Attribute(type, identifier));
        }

        public ObservableCollection<Attribute> Attributes { get => attributes; set => attributes = value; }

        private void test_data()
        {
            attributes.Add(new Attribute(Type.TYPE_INT, "a"));
            attributes.Add(new Attribute(Type.TYPE_BOOL, "b"));
        }
    }
}
