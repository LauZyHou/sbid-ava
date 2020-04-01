using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._M
{
    // Label列表对应Content列表，用于压缩SecurityPolicy在JSON中的存储
    public class LabelsContentsPair
    {
        public List<string> Labels { get; set; }
        public List<string> Contents { get; set; }
    }
}
