using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    // 类图面板
    public class ClassDiagram_P_VM : SidePanel_VM
    {
        private static int _id = 1;

        public ClassDiagram_P_VM()
        {
            Name = "概览" + _id;
            _id++;
            test_data();
        }

        private void test_data()
        {
            NetworkItemVMs.Add(
                new UserType_VM(sbid._M.Type.TYPE_INT)
                {
                    X = 50,
                    Y = 20
                });
            NetworkItemVMs.Add(
                new UserType_VM(sbid._M.Type.TYPE_BOOL)
                {
                    X = 230,
                    Y = 20
                });
        }

        #region 按钮和右键菜单命令

        // 创建自定义类型
        public void CreateUserType()
        {
            UserType_VM userTypeVM = new UserType_VM();
            NetworkItemVMs.Add(userTypeVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的自定义类型：" + userTypeVM.Type.Name;
        }

        #endregion
    }
}
