using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._M
{
    // 数组类型的实例(可以是引用类型数组，也可以是值类型数组)
    public class ArrayInstance : Instance
    {
        private ObservableCollection<Instance> arrayItems = new ObservableCollection<Instance>();
        private int index = 0; // 用来产生数组下标

        public ArrayInstance(Attribute attribute)
            : base(attribute)
        {
        }

        // 数组中存放的项，虽说是Instacne，但实际只放ReferenceInstance或者ValueInstance
        public ObservableCollection<Instance> ArrayItems { get => arrayItems; set => arrayItems = value; }

        #region 按钮命令

        // 添加数组项
        public void AddItem()
        {
            // 注意这里创建的是临时Attribute
            Attribute attribute = new Attribute(
                this.Type,
                this.Identifier + "[" + index++ + "]",
                false,
                true
            );
            Instance instance;
            if (attribute.Type is UserType) // 数组里存引用类型项
            {
                instance = ReferenceInstance.build(attribute);
            }
            else // 数组里存值类型项
            {
                instance = new ValueInstance(attribute);
            }
            this.arrayItems.Add(instance);
        }

        // 删除末尾的数组项
        public void RemoveItem()
        {
            int arrayLen = this.arrayItems.Count;
            if (arrayLen > 0)
            {
                this.arrayItems.RemoveAt(arrayLen - 1);
                this.index--;
            }
        }

        #endregion
    }
}
