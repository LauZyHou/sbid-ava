using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class UserType_P_VM : SidePanel_VM
    {
        private ObservableCollection<UserType_VM> userTypeVMs = new ObservableCollection<UserType_VM>();

        public UserType_P_VM() : base("数据类型")
        {
            // 添加int和bool
            userTypeVMs.Add(
                new UserType_VM(sbid._M.Type.TYPE_INT)
                {
                    X = 50,
                    Y = 20
                });
            userTypeVMs.Add(
                new UserType_VM(sbid._M.Type.TYPE_BOOL)
                {
                    X = 230,
                    Y = 20
                });
        }

        // 集成协议下所有的UserType
        public ObservableCollection<UserType_VM> UserTypeVMs { get => userTypeVMs; set => userTypeVMs = value; }

        // 创建新的UserType
        public void CreateUserType()
        {
            userTypeVMs.Add(new UserType_VM() { 
                X = 400,
                Y = 20
            });
            ResourceManager.mainWindowVM.Tips = "创建了新的UserType";
        }
    }
}
