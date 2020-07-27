using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    /* 
    属性导航用于SafetyProperty的编辑窗体中的属性导航器
    因为对数组而言，需要Type identifier[ 写下标的文本框 ]，然后直接展开
    所以这里需要重新写一组属性导航的类，而不能直接用Instance下面的类实现
    而且缩减到引用类型和值类型两种，数组用IsArray字段判断，不需要ArrayNav
    */
    // 属性导航的基类，对标Instance类
    public abstract class Nav : ReactiveObject
    {
        private readonly Type type;
        private readonly string identifier;
        private readonly bool isArray;
        private readonly Nav parentNav;
        private string arrayIndex = "0";

        protected Nav(Type type, string identifier, bool isArray, Nav parentNav)
        {
            this.type = type;
            this.identifier = identifier;
            this.isArray = isArray;
            this.parentNav = parentNav;
        }

        protected Nav(Attribute attribute, Nav parentNav)
        {
            this.type = attribute.Type;
            this.identifier = attribute.Identifier;
            this.isArray = attribute.IsArray;
            this.parentNav = parentNav;
        }

        public Type Type => type;
        public string Identifier => identifier;
        public bool IsArray => isArray;
        // 指向父结点，如果父节点是Process，就存null
        public Nav ParentNav => parentNav;
        // 如果是数组，这里表达下标索引，默认是"0"，用户可以更改
        public string ArrayIndex { get => arrayIndex; set => this.RaiseAndSetIfChanged(ref arrayIndex, value); }

        // 这里不区分数组
        public override string ToString()
        {
            return type.Name + " " + identifier;
        }
    }
}
