using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using sbid._M;
using sbid._VM;
using System.Collections.ObjectModel;

namespace sbid._V.Process
{
    public class Process_EW_V : Window
    {
        #region 构造

        public Process_EW_V()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            // 初始化.cs文件中的数据绑定
            init_binding();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #endregion

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
            ((Process_EW_VM)DataContext).Process.Attributes.Add(attribute);
            ResourceManager.mainWindowVM.Tips = "为进程模板[" + ((Process_EW_VM)DataContext).Process.Name + "]添加了成员变量：" + attribute;
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
            ResourceManager.mainWindowVM.Tips = "为进程模板[" + ((Process_EW_VM)DataContext).Process.Name + "]更新了成员变量：" + attribute;
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
            ((Process_EW_VM)DataContext).Process.Attributes.Remove(attribute);
            ResourceManager.mainWindowVM.Tips = "为进程模板[" + ((Process_EW_VM)DataContext).Process.Name + "]删除了成员变量：" + attribute;
        }

        public void Add_NZMethod()
        {
            ListBox innerMethod_ListBox = ControlExtensions.FindControl<ListBox>(this, "innerMethod_ListBox");
            if (innerMethod_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定内置方法！";
                return;
            }

            ComboBox crypto_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "crypto_ComboBox");
            if (crypto_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定加密算法！";
                return;
            }

            // todo 方法判重

            Method method_template = (Method)innerMethod_ListBox.SelectedItem;
            // 形参表要拷贝一份,以防止在自定Method中对其修改时影响到内置Method
            ObservableCollection<Attribute> paramerters = new ObservableCollection<Attribute>();
            foreach (Attribute attribute in method_template.Parameters)
            {
                paramerters.Add(new Attribute(attribute.Type, attribute.Identifier));
            }
            // 这里加密方法要用用户选中的,而不是内置方法模板里的None
            Method method = new Method(method_template.ReturnType, method_template.Name, paramerters, (Crypto)crypto_ComboBox.SelectedItem);
            ((Process_EW_VM)DataContext).Process.Methods.Add(method);
            ResourceManager.mainWindowVM.Tips = "为进程模板[" + ((Process_EW_VM)DataContext).Process.Name + "]添加了成员方法：" + method;
        }

        public void Update_NZMethod()
        {
            ListBox method_NZ_ListBox = ControlExtensions.FindControl<ListBox>(this, "method_NZ_ListBox");
            if (method_NZ_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要修改的Method！";
                return;
            }

            ListBox innerMethod_ListBox = ControlExtensions.FindControl<ListBox>(this, "innerMethod_ListBox");
            if (innerMethod_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定内置方法！";
                return;
            }

            ComboBox crypto_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "crypto_ComboBox");
            if (crypto_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定加密算法！";
                return;
            }

            // todo 方法判重

            Method method_template = (Method)innerMethod_ListBox.SelectedItem;
            ObservableCollection<Attribute> paramerters = new ObservableCollection<Attribute>();
            foreach (Attribute attribute in method_template.Parameters)
            {
                paramerters.Add(new Attribute(attribute.Type, attribute.Identifier));
            }
            //Method method = (Method)method_NZ_ListBox.SelectedItem;
            //method.ReturnType = method_template.ReturnType;
            //method.Name = method_template.Name;
            //method.Parameters = paramerters;
            //method.CryptoSuffix = (Crypto)crypto_ComboBox.SelectedItem;
            Method method = new Method(
                method_template.ReturnType,
                method_template.Name,
                paramerters,
                (Crypto)crypto_ComboBox.SelectedItem
            );
            ((Process_EW_VM)DataContext).Process.Methods[method_NZ_ListBox.SelectedIndex] = method;
            ResourceManager.mainWindowVM.Tips = "为进程模板[" + ((Process_EW_VM)DataContext).Process.Name + "]更新了成员方法：" + method;
            /*
             注：
             如果用上面注释掉的方式对Method直接进行修改
             Method本身能获取各个属性修改的通知
             但是Process没法获取到Methods列表修改的通知
             除非在数据模板里不用ToString()或者多源绑定
             而是用StackPanel里装Method的每个字段
             但这样写起来比较麻烦
             这里创建一个新的Method,然后在列表里把那一项改掉
             实际上是利用了ObervableCollection能在成员修改时发出通知
             使得Process获取到了这个通知
             */
        }

        public void Delete_NZMethod()
        {
            ListBox method_NZ_ListBox = ControlExtensions.FindControl<ListBox>(this, "method_NZ_ListBox");
            if (method_NZ_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要删除的Method！";
                return;
            }

            Method method = (Method)method_NZ_ListBox.SelectedItem;
            ((Process_EW_VM)DataContext).Process.Methods.Remove(method);
            ResourceManager.mainWindowVM.Tips = "为进程模板[" + ((Process_EW_VM)DataContext).Process.Name + "]删除了成员方法：" + method;
        }

        #endregion

        #region 私有

        // 初始化.cs文件中的数据绑定,一些不方便在xaml中绑定的部分在这里绑定
        private void init_binding()
        {
            // 绑定Method的内置方法
            ListBox innerMethod_ListBox = ControlExtensions.FindControl<ListBox>(this, "innerMethod_ListBox");
            innerMethod_ListBox.Items = Method.InnerMethods;

            // 绑定Method的加密算法
            ComboBox crypto_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "crypto_ComboBox");
            crypto_ComboBox.Items = System.Enum.GetValues(typeof(Crypto));
            crypto_ComboBox.SelectedItem = Crypto.None;
        }

        #endregion
    }
}
