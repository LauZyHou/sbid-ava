using ReactiveUI;
using sbid._M;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class UserType_EW_VM : ViewModelBase
    {
        private UserType userType;
        private ObservableCollection<Type> types = new ObservableCollection<_M.Type>();
        private ObservableCollection<Attribute> @params = new ObservableCollection<Attribute>();
        private ObservableCollection<UserType> userTypes = new ObservableCollection<UserType>();

        // 自己这个UserType
        public UserType UserType { get => userType; set => userType = value; }
        // 除自己集成的Type外的所有Type对象(包括int,bool),作为可添加的Attribute的可选类型
        public ObservableCollection<Type> Types { get => types; set => types = value; }
        // 除自己集成的Type外的所有UserType对象,作为可选的继承目标
        public ObservableCollection<UserType> UserTypes { get => userTypes; set => userTypes = value; }

        // Method参数列表绑定此处
        public ObservableCollection<Attribute> Params { get => @params; set => this.RaiseAndSetIfChanged(ref @params, value); }

        #region 按钮命令

        // 设置为不继承，即将Parent指向null
        public void Clear_Parent()
        {
            if (userType.Parent == null)
            {
                ResourceManager.mainWindowVM.Tips = "已经没有继承关系了，无需重复操作";
                return;
            }
            userType.Parent = null;
            ResourceManager.mainWindowVM.Tips = "已清除继承关系";
        }

        #endregion
    }
}
