using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // 引用类型的实例
    public class ReferenceInstance : Instance
    {
        private List<Instance> properties = new List<Instance>();

        public ReferenceInstance(Type type, string identifier) : base(type, identifier)
        {
        }

        // 因为是引用类型，所以会有属性列表
        public List<Instance> Properties { get => properties; }
    }
}
