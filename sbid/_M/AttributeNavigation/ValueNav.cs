using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    public class ValueNav : Nav
    {
        public ValueNav(Type type, string identifier, bool isArray, Nav parentNav)
            : base(type, identifier, isArray, parentNav)
        {
        }

        public ValueNav(Attribute attribute, Nav parentNav)
            : base(attribute, parentNav)
        {
        }

        // 因为只是做属性导航，ValueNav里不需要真的存Value的，这和ValueInstance不同
    }
}
