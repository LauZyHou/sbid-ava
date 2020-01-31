using System;
using sbid._M;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;

namespace sbid._VM
{
    public class UserType_VM : NetworkNode
    {
        private static int _id = 1;
        private sbid._M.Type type;

        // 无参构造时,构造的总是UserType
        public UserType_VM()
        {
            type = new UserType("未命名" + _id);
            _id++;
        }

        // 传入Type参数的构造,当为内置的int和bool创建UserType_VM时使用此构造
        public UserType_VM(sbid._M.Type type)
        {
            this.type = type;
        }

        // 因为UserType_VM也可能维护底层的int和bool,所以这里用Type而不是UserType
        public sbid._M.Type Type { get => type; set => type = value; }
    }
}
