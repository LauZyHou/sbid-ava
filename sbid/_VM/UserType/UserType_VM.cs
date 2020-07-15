using sbid._M;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;
using sbid._V;
using Avalonia.Controls;

namespace sbid._VM
{
    public class UserType_VM : NetworkItem_VM
    {
        private Type type;

        // 无参构造时,构造的总是UserType
        public UserType_VM()
        {
            this.type = new UserType();
            // 为了防止新创建的UserType和用户已有的重名
            // 这里检查一下重名，如果重名了就在后面补一个随机的字母
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            System.Random random = new System.Random();
            while (!ResourceManager.checkUserTypeName((UserType)type, type.Name))
            {
                type.Name += letters[random.Next(52)];
            }
        }

        // 传入Type参数的构造,当为内置的Type创建UserType_VM时使用此构造
        public UserType_VM(Type type)
        {
            this.type = type;
        }

        // 因为UserType_VM也可能维护底层的int和bool,所以这里用Type而不是UserType
        public Type Type { get => type; set => type = value; }

        #region 右键菜单命令

        // 尝试删除当前UserType_VM
        public void DeleteUserTypeVM()
        {
            if (!(type is UserType) || type == Type.TYPE_TIMER || type == Type.TYPE_BYTE_VEC)
            {
                ResourceManager.mainWindowVM.Tips = "无效的操作，禁止删除内置类型：" + type.Name + "！";
                return;
            }
            ResourceManager.mainWindowVM.SelectedItem.SelectedItem.SelectedItem.UserControlVMs.Remove(this);
            ResourceManager.mainWindowVM.Tips = "删除了自定义类型：" + type.Name;
        }

        // 尝试打开当前UserType_VM的编辑窗体
        public void EditUserTypeVM()
        {
            if (!(type is UserType) || type == Type.TYPE_TIMER || type == Type.TYPE_BYTE_VEC)
            {
                ResourceManager.mainWindowVM.Tips = "无效的操作，禁止编辑内置类型：" + type.Name + "！";
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
            foreach (NetworkItem_VM item in ResourceManager.mainWindowVM.SelectedItem.SelectedItem.SelectedItem.UserControlVMs)
            {
                if (item is UserType_VM && item != this)
                {
                    ((UserType_EW_VM)userTypeEWV.DataContext).Types.Add(((UserType_VM)item).Type);
                    // 如果是UserType的话要加到相应列表里
                    if (((UserType_VM)item).Type is UserType)
                    {
                        UserType userType = (UserType)((UserType_VM)item).Type;
                        ((UserType_EW_VM)userTypeEWV.DataContext).UserTypes.Add(userType);
                    }
                }
            }

            // [bugfix]因为在xaml里绑定UserType打开编辑窗口显示出不来，只好在这里手动设置一下
            ComboBox userType_ComboBox = ControlExtensions.FindControl<ComboBox>(userTypeEWV, "userType_ComboBox");
            userType_ComboBox.SelectedItem = ((UserType_EW_VM)userTypeEWV.DataContext).UserType.Parent;

            userTypeEWV.ShowDialog(ResourceManager.mainWindowV);
            ResourceManager.mainWindowVM.Tips = "打开了自定义类型：" + type.Name + "的编辑窗体";
        }

        #endregion
    }
}
