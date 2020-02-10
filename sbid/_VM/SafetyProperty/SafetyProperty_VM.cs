using sbid._M;
using sbid._V;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class SafetyProperty_VM : NetworkItem_VM
    {
        public static int _id = 1;
        private SafetyProperty safetyProperty;

        public SafetyProperty_VM()
        {
            safetyProperty = new SafetyProperty("未命名" + _id);
            _id++;
        }

        public SafetyProperty SafetyProperty { get => safetyProperty; set => safetyProperty = value; }

        #region 右键菜单命令

        // 尝试打开当前SafetyProperty_VM的编辑窗体
        public void EditSafetyPropertyVM()
        {
            // 从主窗体打开编辑窗体,并在其DataContext中集成当前SafetyProperty_VM里集成的SafetyProperty对象,以能对其作修改
            SafetyProperty_EW_V safetyPropertyEWV = new SafetyProperty_EW_V()
            {
                DataContext = new SafetyProperty_EW_VM()
                {
                    SafetyProperty = safetyProperty
                }
            };

            safetyPropertyEWV.ShowDialog(ResourceManager.mainWindowV);
            ResourceManager.mainWindowVM.Tips = "打开了SafetyProperty[" + safetyProperty.Name + "]的编辑窗体";
        }

        #endregion
    }
}
