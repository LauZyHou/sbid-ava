using sbid._M;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class PropertyVerify_SW_VM : ViewModelBase
    {
        public ObservableCollection<PropertyVerified> PropertiesVerified { get; }

        public PropertyVerify_SW_VM()
        {
            PropertiesVerified = new ObservableCollection<PropertyVerified>(GenerateMockPropertiesVerified());
        }

        private IEnumerable<PropertyVerified> GenerateMockPropertiesVerified()
        {
            List<PropertyVerified> defaultPropertiesVerified = new List<PropertyVerified>()
            {
                new PropertyVerified("机密性", "P1.msg", true, "-"),
                new PropertyVerified("可用性", "P2.State6", true, "-"),
                new PropertyVerified("完整性", "P1.s1.msg | P2.s2.msg", false, "-"),
                new PropertyVerified("机密性", "P1.S1.msg.auth | P2.S2.msg.auth", true, "-")
            };
            return defaultPropertiesVerified;
        }
    }
}
