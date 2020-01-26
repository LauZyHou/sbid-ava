using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // 普适的类型类,基本类型int,bool也属于此类
    public class Type
    {
        private string name;

        // 类型名
        public string Name { get => name; set => name = value; }
    }
}
