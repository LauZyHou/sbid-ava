using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._M
{
    // 引用数组类型的实例
    class ReferenceArrayInstance : Instance
    {
        private ObservableCollection<ReferenceInstance> arrayItems = new ObservableCollection<ReferenceInstance>();
        private int index = 0; // 用来产生数组下标

        public ReferenceArrayInstance(Attribute attribute)
            : base(attribute)
        {
        }

        // 引用类型数组的数组项列表
        public ObservableCollection<ReferenceInstance> ArrayItems { get => arrayItems; }

        #region 按钮命令

        // 添加数组项
        public void AddItem()
        {
            Attribute attribute = new Attribute(this.Type, this.Identifier + "[" + index++ + "]");
            ReferenceInstance referenceInstance = ReferenceInstance.build(attribute);
            this.arrayItems.Add(referenceInstance);
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
