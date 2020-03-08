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

        // 自己这个UserType
        public UserType UserType { get => userType; set => userType = value; }
        // 除自己集成的Type外的所有Type对象(包括int,bool),作为可添加的Attribute的可选类型
        public ObservableCollection<Type> Types { get => types; set => types = value; }

        // Method参数列表绑定此处
        public ObservableCollection<Attribute> Params { get => @params; set => this.RaiseAndSetIfChanged(ref @params, value); }
    }
}
