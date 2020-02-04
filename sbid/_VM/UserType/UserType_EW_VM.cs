using sbid._M;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class UserType_EW_VM
    {
        private UserType userType;
        private ObservableCollection<_M.Type> types = new ObservableCollection<_M.Type>();

        // 自己这个UserType
        public UserType UserType { get => userType; set => userType = value; }
        // 除自己集成的Type外的所有Type对象(包括int,bool),作为可添加的Attribute的可选类型
        public ObservableCollection<sbid._M.Type> Types { get => types; set => types = value; }
    }
}
