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

        // 因为是引用类型，所以会有属性列表
        public List<Instance> Properties { get => properties; }

        #region 静态

        // 从Attribute=<UserType,string,bool>递归构造ReferenceInstance
        public static ReferenceInstance build(Attribute attribute)
        {
            ReferenceInstance referenceInstance = new ReferenceInstance(attribute);
            UserType userType = (UserType)attribute.Type;
            // 从自己的Attribute构造Instance列表
            foreach (Attribute attr in userType.Attributes)
            {
                Instance instance;
                if (attr.IsArray) // 数组
                {
                    instance = new ArrayInstance(attr);
                }
                else if (attr.Type is UserType) // 引用类型
                {
                    instance = ReferenceInstance.build(attr);
                }
                else // 值类型
                {
                    instance = new ValueInstance(attr);
                }
                referenceInstance.properties.Add(instance);
            }
            // 从祖先类的Attribute构造Instance列表
            UserType pointer = userType.Parent;
            while (pointer != null)
            {
                foreach (Attribute attr in pointer.Attributes)
                {
                    Instance instance;
                    if (attr.IsArray) // 数组
                    {
                        instance = new ArrayInstance(attr);
                    }
                    else if (attr.Type is UserType) // 引用类型
                    {
                        instance = ReferenceInstance.build(attr);
                    }
                    else // 值类型
                    {
                        instance = new ValueInstance(attr);
                    }
                    referenceInstance.properties.Add(instance);
                }
                pointer = pointer.Parent;
            }
            // 构造完将这个列表返回
            return referenceInstance;
        }

        #endregion
    }
}
