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

        public UserType() : base()
        {
            //test_data();
        }

        public ObservableCollection<Attribute> Attributes { get => attributes; set => attributes = value; }

        private void test_data()
        {
            attributes.Add(new Attribute(Type.TYPE_INT, "a"));
            attributes.Add(new Attribute(Type.TYPE_BOOL, "b"));
        }
    }
}
