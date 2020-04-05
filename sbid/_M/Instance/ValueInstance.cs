using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // 值类型的实例
    public class ValueInstance : Instance
    {
        private string _value = "a";

        public ValueInstance(Type type, string identifier) : base(type, identifier)
        {
        }

        // 因为是基本类型，所以有一个值，这里用字符串模拟
        public string Value { get => _value; set => this.RaiseAndSetIfChanged(ref _value, value); }
    }
}
