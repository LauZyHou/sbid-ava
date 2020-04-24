using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace sbid._M
{
    // 引用类型的实例
    public class ReferenceInstance : Instance
    {
        private List<Instance> properties = new List<Instance>();

        public ReferenceInstance(Attribute attribute)
            : base(attribute)
        {
        }

        public ReferenceInstance(UserType userType, string identifier, bool isArray)
            : base(userType, identifier, isArray)
        {
        }

        // 因为是引用类型，所以会有属性列表
        public List<Instance> Properties { get => properties; }

        #region 静态

        // 从Attribute=<UserType,string,bool>递归构造ReferenceInstance
        public static ReferenceInstance build(UserType userType, string identifier)
        {
            ReferenceInstance referenceInstance = new ReferenceInstance(userType, identifier, false);
            foreach (Attribute attribute in userType.Attributes)
            {
                Instance instance = null;
                if (attribute.Type is UserType)
                {
                    instance = ReferenceInstance.build((UserType)attribute.Type, attribute.Identifier);
                }
                else
                {
                    instance = new ValueInstance(attribute.Type, attribute.Identifier, false);
                }
                referenceInstance.properties.Add(instance);
            }
            return referenceInstance;
        }

        #endregion
    }
}
