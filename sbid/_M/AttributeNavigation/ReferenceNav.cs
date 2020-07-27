using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    public class ReferenceNav : Nav
    {
        private readonly List<Nav> childrenNav = new List<Nav>();

        public ReferenceNav(Type type, string identifier, bool isArray, Nav parentNav)
            : base(type, identifier, isArray, parentNav)
        {
        }

        public ReferenceNav(Attribute attribute, Nav parentNav)
            : base(attribute, parentNav)
        {
        }

        public List<Nav> ChildrenNav => childrenNav;

        #region 静态

        // 从Attribute=<UserType,string,bool>递归构造ReferenceNav
        public static ReferenceNav build(Attribute attribute, Nav parentNav)
        {
            ReferenceNav referenceNav = new ReferenceNav(attribute, parentNav);
            UserType userType = (UserType)attribute.Type;
            // 从自己的Attribute构造Nav列表
            foreach (Attribute attr in userType.Attributes)
            {
                Nav nav;
                if (attr.Type is UserType) // 引用类型
                {
                    nav = ReferenceNav.build(attr, referenceNav);
                }
                else // 值类型
                {
                    nav = new ValueNav(attr, referenceNav);
                }
                referenceNav.childrenNav.Add(nav);
            }
            // 从祖先类的Attribute构造Nav列表
            UserType pointer = userType.Parent;
            while (pointer != null)
            {
                foreach (Attribute attr in pointer.Attributes)
                {
                    Nav nav;
                    if (attr.Type is UserType) // 引用类型
                    {
                        nav = ReferenceNav.build(attr, referenceNav);
                    }
                    else // 值类型
                    {
                        nav = new ValueNav(attr, referenceNav);
                    }
                    referenceNav.childrenNav.Add(nav);
                }
                pointer = pointer.Parent;
            }
            // 构造完将它返回
            return referenceNav;
        }

        #endregion
    }
}
