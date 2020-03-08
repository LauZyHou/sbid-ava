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
            this.init_event();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #region 按钮命令

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

            // todo 变量名判重

            Attribute attribute = new Attribute((sbid._M.Type)type_ListBox.SelectedItem, attrId_TextBox.Text);
            ((UserType_EW_VM)DataContext).UserType.Attributes.Add(attribute);
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

            TextBox attrId_TextBox = ControlExtensions.FindControl<TextBox>(this, "attrId_TextBox");
            if (attrId_TextBox.Text == null || attrId_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要给出成员名称！";
                return;
            }

            // todo 变量名判重

            Attribute attribute = ((Attribute)attr_ListBox.SelectedItem);
            attribute.Type = (sbid._M.Type)type_ListBox.SelectedItem;
            attribute.Identifier = attrId_TextBox.Text;
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

            Attribute attribute = new Attribute((Type)paramType_ComboBox.SelectedItem, paramName_TextBox.Text);
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

            TextBox paramName_TextBox = ControlExtensions.FindControl<TextBox>(this, "paramName_TextBox");
            if (paramName_TextBox.Text == null || paramName_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要提供参数名称！";
                return;
            }

            Attribute attribute = (Attribute)param_ListBox.SelectedItem;
            attribute.Type = (Type)paramType_ComboBox.SelectedItem;
            attribute.Identifier = paramName_TextBox.Text;
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

            ObservableCollection<Attribute> parameters = ((UserType_EW_VM)DataContext).Params;
            if (parameters.Count == 0)
            {
                ResourceManager.mainWindowVM.Tips = "至少要在形参表中添加一个参数！";
                return;
            }

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

            ObservableCollection<Attribute> parameters = ((UserType_EW_VM)DataContext).Params;
            if (parameters.Count == 0)
            {
                ResourceManager.mainWindowVM.Tips = "至少要在形参表中添加一个参数！";
                return;
            }

            Method method = new Method(
                (Type)returnType_ComboBox.SelectedItem,
                methodName_TextBox.Text,
                parameters,
                Crypto.None
            );
            ((UserType_EW_VM)DataContext).UserType.Methods[method_ListBox.SelectedIndex] = method;
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
                ((UserType_EW_VM)DataContext).Params.Add(new Attribute(attribute.Type, attribute.Identifier));
            }
            ResourceManager.mainWindowVM.Tips = "选中了Method：" + (Method)method_ListBox.SelectedItem + "，已拷贝其参数列表";
        }

        #endregion

        #region 初始化

        // 初始化.cs文件中的事件处理方法,一些无法在xaml中绑定的部分在这里绑定
        private void init_event()
        {
            // 绑定Method右侧列表选中项变化的处理方法
            ListBox method_ListBox = ControlExtensions.FindControl<ListBox>(this, "method_ListBox");
            method_ListBox.SelectionChanged += method_ListBox_Changed;
        }

        #endregion
    }
}
