using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using sbid._M;
using sbid._VM;
using System.Collections.ObjectModel;

namespace sbid._V
{
    public class UserType_EW_V : Window
    {

        public UserType_EW_V()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            init_event();
            get_control_reference();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #region 辅助构造

        // 初始化.cs文件中的事件处理方法,一些无法在xaml中绑定的部分在这里绑定
        private void init_event()
        {
            // 绑定Method右侧列表选中项变化的处理方法
            ListBox method_ListBox = ControlExtensions.FindControl<ListBox>(this, "method_ListBox");
            method_ListBox.SelectionChanged += method_ListBox_Changed;
        }

        #endregion

        #region 按钮命令

        public void Update_Name()
        {
            if (name_TextBox.Text == null || name_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要给出类型名称！";
                return;
            }

            // 检查类型名称独一无二
            if (Checker.UserType_Name_Repeat(VM.UserType, name_TextBox.Text))
            {
                ResourceManager.mainWindowVM.Tips = "不可用的名称，与其它UserType重名！";
                return;
            }

            VM.UserType.Name = name_TextBox.Text;
            ResourceManager.mainWindowVM.Tips = "为自定义类型设置了名称：" + VM.UserType.Name;
        }

        // 写入继承关系
        public void Set_Parent()
        {
            ComboBox userType_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "userType_ComboBox");
            if (userType_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定UserType！";
                return;
            }
            // 选中的UserType
            UserType userType = (UserType)userType_ComboBox.SelectedItem;
            // 当前UserType
            UserType nowUserType = ((UserType_EW_VM)DataContext).UserType;

            // 判断环形继承
            if (JudgeLoopExtend(userType))
            {
                string way = userType.Parent == nowUserType ? "直接" : "间接";
                ResourceManager.mainWindowVM.Tips = "禁止！该操作会引起环形继承，因为" + userType.Name + way + "继承了" + nowUserType.Name;
                return;
            }

            // 设置继承关系
            nowUserType.Parent = userType;
            ResourceManager.mainWindowVM.Tips = "写入继承关系，使继承自UserType：" + userType.Name;
        }

        public void Add_Attribute()
        {
            ListBox type_ListBox = ControlExtensions.FindControl<ListBox>(this, "type_ListBox");
            if (type_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定成员类型！";
                return;
            }

            TextBox attrId_TextBox = ControlExtensions.FindControl<TextBox>(this, "attrId_TextBox");
            if (attrId_TextBox.Text == null || attrId_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要给出成员名称！";
                return;
            }

            if (attr_IsArray_CheckBox.IsChecked == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定是否是数组！";
                return;
            }

            if (Checker.UserType_Contain_PropName(VM.UserType, attrId_TextBox.Text))
            {
                ResourceManager.mainWindowVM.Tips = "标识符重复！";
                return;
            }

            Attribute attribute = new Attribute(
                (sbid._M.Type)type_ListBox.SelectedItem,
                attrId_TextBox.Text,
                (bool)attr_IsArray_CheckBox.IsChecked
            );
            VM.UserType.Attributes.Add(attribute);
            ResourceManager.mainWindowVM.Tips = "为自定义类型[" + ((UserType_EW_VM)DataContext).UserType.Name + "]添加了成员变量：" + attribute;
        }

        public void Update_Attribute()
        {
            ListBox attr_ListBox = ControlExtensions.FindControl<ListBox>(this, "attr_ListBox");
            if (attr_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要修改的Attribute！";
                return;
            }

            ListBox type_ListBox = ControlExtensions.FindControl<ListBox>(this, "type_ListBox");
            if (type_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定成员类型！";
                return;
            }

            if (attr_IsArray_CheckBox.IsChecked == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定是否是数组！";
                return;
            }

            TextBox attrId_TextBox = ControlExtensions.FindControl<TextBox>(this, "attrId_TextBox");
            if (attrId_TextBox.Text == null || attrId_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要给出成员名称！";
                return;
            }

            Attribute attribute = ((Attribute)attr_ListBox.SelectedItem);

            if (attrId_TextBox.Text != attribute.Identifier && Checker.UserType_Contain_PropName(VM.UserType, attrId_TextBox.Text))
            {
                ResourceManager.mainWindowVM.Tips = "标识符重复！";
                return;
            }

            attribute.Type = (sbid._M.Type)type_ListBox.SelectedItem;
            attribute.Identifier = attrId_TextBox.Text;
            attribute.IsArray = (bool)attr_IsArray_CheckBox.IsChecked;
            ResourceManager.mainWindowVM.Tips = "为自定义类型[" + ((UserType_EW_VM)DataContext).UserType.Name + "]更新了成员变量：" + attribute;
        }

        public void Delete_Attribute()
        {
            ListBox attr_ListBox = ControlExtensions.FindControl<ListBox>(this, "attr_ListBox");
            if (attr_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要删除的Attribute！";
                return;
            }

            Attribute attribute = (Attribute)attr_ListBox.SelectedItem;
            ((UserType_EW_VM)DataContext).UserType.Attributes.Remove(attribute);
            ResourceManager.mainWindowVM.Tips = "为自定义类型[" + ((UserType_EW_VM)DataContext).UserType.Name + "]删除了成员变量：" + attribute;
        }

        public void Add_Param()
        {
            ComboBox paramType_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "paramType_ComboBox");
            if (paramType_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定参数类型！";
                return;
            }

            TextBox paramName_TextBox = ControlExtensions.FindControl<TextBox>(this, "paramName_TextBox");
            if (paramName_TextBox.Text == null || paramName_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要提供参数名称！";
                return;
            }

            if (method_param_IsArray_CheckBox.IsChecked == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定是否是数组！";
                return;
            }

            ListBox param_ListBox = ControlExtensions.FindControl<ListBox>(this, "param_ListBox");
            if (Checker.ParamList_Contain_Name(param_ListBox.Items, paramName_TextBox.Text))
            {
                ResourceManager.mainWindowVM.Tips = "重复的参数名！";
                return;
            }

            Attribute attribute = new Attribute(
                (Type)paramType_ComboBox.SelectedItem,
                paramName_TextBox.Text,
                (bool)method_param_IsArray_CheckBox.IsChecked
            );
            ((UserType_EW_VM)DataContext).Params.Add(attribute);
            ResourceManager.mainWindowVM.Tips = "已在临时参数列表中添加参数：" + attribute;
        }

        public void Update_Param()
        {
            ListBox param_ListBox = ControlExtensions.FindControl<ListBox>(this, "param_ListBox");
            if (param_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要在临时参数列表中选定要修改的参数！";
                return;
            }

            ComboBox paramType_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "paramType_ComboBox");
            if (paramType_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定参数类型！";
                return;
            }

            if (method_param_IsArray_CheckBox.IsChecked == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定是否是数组！";
                return;
            }

            TextBox paramName_TextBox = ControlExtensions.FindControl<TextBox>(this, "paramName_TextBox");
            if (paramName_TextBox.Text == null || paramName_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要提供参数名称！";
                return;
            }

            Attribute attribute = (Attribute)param_ListBox.SelectedItem;

            if (attribute.Identifier != paramName_TextBox.Text && Checker.ParamList_Contain_Name(param_ListBox.Items, paramName_TextBox.Text))
            {
                ResourceManager.mainWindowVM.Tips = "重复的参数名！";
                return;
            }

            attribute.Type = (Type)paramType_ComboBox.SelectedItem;
            attribute.Identifier = paramName_TextBox.Text;
            attribute.IsArray = (bool)method_param_IsArray_CheckBox.IsChecked;
            ResourceManager.mainWindowVM.Tips = "已在临时参数列表中更新参数：" + attribute;
        }

        public void Delete_Param()
        {
            ListBox param_ListBox = ControlExtensions.FindControl<ListBox>(this, "param_ListBox");
            if (param_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要在临时参数列表中选定要删除的参数！";
                return;
            }

            Attribute attribute = (Attribute)param_ListBox.SelectedItem;
            ((UserType_EW_VM)DataContext).Params.Remove(attribute);
            ResourceManager.mainWindowVM.Tips = "已在临时参数列表中删除参数：" + attribute;
        }

        public void Add_Method()
        {
            ComboBox returnType_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "returnType_ComboBox");
            if (returnType_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定自定Method的返回类型！";
                return;
            }

            TextBox methodName_TextBox = ControlExtensions.FindControl<TextBox>(this, "methodName_TextBox");
            if (methodName_TextBox.Text == null || methodName_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要给出自定Method的方法名！";
                return;
            }

            if (Checker.UserType_Contain_PropName(VM.UserType, methodName_TextBox.Text))
            {
                ResourceManager.mainWindowVM.Tips = "标识符重复！";
                return;
            }

            ObservableCollection<Attribute> parameters = ((UserType_EW_VM)DataContext).Params;

            Method method = new Method(
                (Type)returnType_ComboBox.SelectedItem,
                methodName_TextBox.Text,
                parameters,
                Crypto.None
            );

            ((UserType_EW_VM)DataContext).UserType.Methods.Add(method);
            ResourceManager.mainWindowVM.Tips = "添加了自定Method：" + method;

            // 添加完成后,要将临时参数列表拿掉,这样再向临时参数列表中添加/更新内容也不会影响刚刚添加的Method
            ((UserType_EW_VM)DataContext).Params = new ObservableCollection<Attribute>();
        }

        public void Update_Method()
        {
            ListBox method_ListBox = ControlExtensions.FindControl<ListBox>(this, "method_ListBox");
            if (method_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要更新的Method！";
                return;
            }

            ComboBox returnType_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "returnType_ComboBox");
            if (returnType_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定自定Method的返回类型！";
                return;
            }

            TextBox methodName_TextBox = ControlExtensions.FindControl<TextBox>(this, "methodName_TextBox");
            if (methodName_TextBox.Text == null || methodName_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要给出自定Method的方法名！";
                return;
            }

            if (((Method)method_ListBox.SelectedItem).Name != methodName_TextBox.Text && Checker.UserType_Contain_PropName(VM.UserType, methodName_TextBox.Text))
            {
                ResourceManager.mainWindowVM.Tips = "标识符重复！";
                return;
            }

            ObservableCollection<Attribute> parameters = ((UserType_EW_VM)DataContext).Params;

            Method method = new Method(
                (Type)returnType_ComboBox.SelectedItem,
                methodName_TextBox.Text,
                parameters,
                Crypto.None
            );

            string achieve = ((Method)method_ListBox.SelectedItem).Achieve;
            method.Achieve = achieve;

            VM.UserType.Methods[method_ListBox.SelectedIndex] = method;
            ResourceManager.mainWindowVM.Tips = "更新了自定Method：" + method;

            // 更新完成后,要将临时参数列表拿掉,这样再向临时参数列表中添加/更新内容也不会影响刚刚添加的Method
            ((UserType_EW_VM)DataContext).Params = new ObservableCollection<Attribute>();
        }

        public void Delete_Method()
        {
            ListBox method_ListBox = ControlExtensions.FindControl<ListBox>(this, "method_ListBox");
            if (method_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要删除的Method！";
                return;
            }

            Method method = (Method)method_ListBox.SelectedItem;
            ((UserType_EW_VM)DataContext).UserType.Methods.Remove(method);
            ResourceManager.mainWindowVM.Tips = "已删除成员方法：" + method;
        }

        public void Achieve_Method()
        {
            ListBox method_ListBox = ControlExtensions.FindControl<ListBox>(this, "method_ListBox");
            if (method_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要实现的Method！";
                return;
            }

            Method method = (Method)method_ListBox.SelectedItem;

            Method_EW_V userType_Method_EW_V = new Method_EW_V()
            {
                DataContext = new Method_EW_VM()
                {
                    Method = method
                }
            };
            userType_Method_EW_V.ShowDialog(this);
            ResourceManager.mainWindowVM.Tips = "实现方法：" + method;
        }

        #endregion

        #region 事件

        // Method右侧列表选中项变化的处理
        private void method_ListBox_Changed(object sender, SelectionChangedEventArgs e)
        {
            // 将右侧选中项的参数列表拷贝到param_ListBox绑定的Params里
            ListBox method_ListBox = ControlExtensions.FindControl<ListBox>(this, "method_ListBox");
            ((UserType_EW_VM)DataContext).Params = new ObservableCollection<Attribute>();
            foreach (Attribute attribute in ((Method)method_ListBox.SelectedItem).Parameters)
            {
                ((UserType_EW_VM)DataContext).Params.Add(new Attribute(attribute));
            }
            ResourceManager.mainWindowVM.Tips = "选中了Method：" + (Method)method_ListBox.SelectedItem + "，已拷贝其参数列表";
        }

        #endregion

        #region 私有

        // 判断将userType设置为当前类型的父类是否会引起环形继承
        private bool JudgeLoopExtend(UserType userType)
        {
            if (userType == null)
                return false;
            // 当前UserType
            UserType nowUserType = ((UserType_EW_VM)DataContext).UserType;
            // 在20步内判断，防止userType本身有环形继承而无限向上查找
            // 程序正常使用不会使userType出现环路，但是用户可以对项目文件修改
            // 所以错误或恶意的修改项目文件是可以使导入的项目出现环形继承的
            // 但是在导入时检查环形继承又提高了导入代价，仅在此处防止出现死循环
            for (int i = 0; i < 20 && userType != null; i++)
            {
                if (userType == nowUserType)
                    return true;
                userType = userType.Parent;
            }
            return false;
        }

        #endregion

        #region 资源引用

        private TextBox name_TextBox;
        private CheckBox attr_IsArray_CheckBox, method_param_IsArray_CheckBox;

        // 获取控件引用
        private void get_control_reference()
        {
            name_TextBox = ControlExtensions.FindControl<TextBox>(this, nameof(name_TextBox));
            attr_IsArray_CheckBox = ControlExtensions.FindControl<CheckBox>(this, nameof(attr_IsArray_CheckBox));
            method_param_IsArray_CheckBox = ControlExtensions.FindControl<CheckBox>(this, nameof(method_param_IsArray_CheckBox));
            attr_IsArray_CheckBox.IsChecked = method_param_IsArray_CheckBox.IsChecked = false;
        }

        public UserType_EW_VM VM { get => (UserType_EW_VM)DataContext; }

        #endregion
    }
}
