using System;
using sbid._M;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;
using sbid._V;

namespace sbid._VM
{
    public class UserType_VM : NetworkItem_VM
    {
        private static int _id = 1;
        private sbid._M.Type type;

        // 无参构造时,构造的总是UserType
        public UserType_VM()
        {
            type = new UserType("未命名" + _id);
            _id++;
        }

        // 传入Type参数的构造,当为内置的int和bool创建UserType_VM时使用此构造
        public UserType_VM(sbid._M.Type type)
        {
            this.type = type;
        }

        // 因为UserType_VM也可能维护底层的int和bool,所以这里用Type而不是UserType
        public sbid._M.Type Type { get => type; set => type = value; }

        #region 右键菜单命令

        // 尝试删除当前UserType_VM
        public void DeleteUserTypeVM()
        {
            if (type == sbid._M.Type.TYPE_INT || type == sbid._M.Type.TYPE_BOOL)
            {
                ResourceManager.mainWindowVM.Tips = "无效的操作，禁止删除内置类型！";
                return;
            }
            ResourceManager.mainWindowVM.SelectedItem.SelectedItem.SelectedItem.NetworkItemVMs.Remove(this);
            ResourceManager.mainWindowVM.Tips = "删除了自定义类型：" + type.Name;
        }

        // 尝试打开当前UserType_VM的编辑窗体
        public void EditUserTypeVM()
        {
            if (type == sbid._M.Type.TYPE_INT || type == sbid._M.Type.TYPE_BOOL)
            {
                ResourceManager.mainWindowVM.Tips = "无效的操作，禁止编辑内置类型！";
                return;
            }

            // 从主窗体打开编辑窗体,并在其DataContext中集成当前UserType_VM里集成的UserType对象,以能对其作修改
            UserType_EW_V userTypeEWV = new UserType_EW_V()
            {
                DataContext = new UserType_EW_VM()
                {
                    UserType = (UserType)type
                }
            };
            // 将所有的Type也传入,作为可添加的Attribute的可选类型
            foreach (NetworkItem_VM item in ResourceManager.mainWindowVM.SelectedItem.SelectedItem.SelectedItem.NetworkItemVMs)
            {
                if (item is UserType_VM && item != this) 
                {
                    ((UserType_EW_VM)userTypeEWV.DataContext).Types.Add(((UserType_VM)item).Type);
                } 
            }

            userTypeEWV.ShowDialog(ResourceManager.mainWindowV);
            ResourceManager.mainWindowVM.Tips = "打开了自定义类型：" + type.Name + "的编辑窗体";
        }

        #endregion
    }
}
