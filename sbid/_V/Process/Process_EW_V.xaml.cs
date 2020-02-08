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
            // 初始化.cs文件中的事件处理
            init_event();
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
             除非在数据模板里不用ToString()
             而是用StackPanel里装Method的每个字段,或者用多源绑定
             但这样写起来比较麻烦(现在觉得就应该用多源绑定)
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
            ResourceManager.mainWindowVM.Tips = "已删除成员方法：" + method;
        }

        public void Add_ZDParam()
        {
            ComboBox paramType_ZD_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "paramType_ZD_ComboBox");
            if (paramType_ZD_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定参数类型！";
                return;
            }

            TextBox paramName_ZD_TextBox = ControlExtensions.FindControl<TextBox>(this, "paramName_ZD_TextBox");
            if (paramName_ZD_TextBox.Text == null || paramName_ZD_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要提供参数名称！";
                return;
            }

            Attribute attribute = new Attribute((Type)paramType_ZD_ComboBox.SelectedItem, paramName_ZD_TextBox.Text);
            ((Process_EW_VM)DataContext).ZDParams.Add(attribute);
            ResourceManager.mainWindowVM.Tips = "已在临时参数列表中添加参数：" + attribute;
        }

        public void Update_ZDParam()
        {
            ListBox param_ZD_ListBox = ControlExtensions.FindControl<ListBox>(this, "param_ZD_ListBox");
            if (param_ZD_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要在临时参数列表中选定要修改的参数！";
                return;
            }

            ComboBox paramType_ZD_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "paramType_ZD_ComboBox");
            if (paramType_ZD_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定参数类型！";
                return;
            }

            TextBox paramName_ZD_TextBox = ControlExtensions.FindControl<TextBox>(this, "paramName_ZD_TextBox");
            if (paramName_ZD_TextBox.Text == null || paramName_ZD_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要提供参数名称！";
                return;
            }

            Attribute attribute = (Attribute)param_ZD_ListBox.SelectedItem;
            attribute.Type = (Type)paramType_ZD_ComboBox.SelectedItem;
            attribute.Identifier = paramName_ZD_TextBox.Text;
            ResourceManager.mainWindowVM.Tips = "已在临时参数列表中更新参数：" + attribute;
        }

        public void Delete_ZDParam()
        {
            ListBox param_ZD_ListBox = ControlExtensions.FindControl<ListBox>(this, "param_ZD_ListBox");
            if (param_ZD_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要在临时参数列表中选定要删除的参数！";
                return;
            }

            Attribute attribute = (Attribute)param_ZD_ListBox.SelectedItem;
            ((Process_EW_VM)DataContext).ZDParams.Remove(attribute);
            ResourceManager.mainWindowVM.Tips = "已在临时参数列表中删除参数：" + attribute;
        }

        public void Add_ZDMethod()
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

            ObservableCollection<Attribute> parameters = ((Process_EW_VM)DataContext).ZDParams;
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

            ((Process_EW_VM)DataContext).Process.Methods.Add(method);
            ResourceManager.mainWindowVM.Tips = "添加了自定Method：" + method;

            // 添加完成后,要将临时参数列表拿掉,这样再向临时参数列表中添加/更新内容也不会影响刚刚添加的Method
            ((Process_EW_VM)DataContext).ZDParams = new ObservableCollection<Attribute>();
        }

        public void Update_ZDMethod()
        {
            ListBox method_ZD_ListBox = ControlExtensions.FindControl<ListBox>(this, "method_ZD_ListBox");
            if (method_ZD_ListBox.SelectedItem == null)
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

            ObservableCollection<Attribute> parameters = ((Process_EW_VM)DataContext).ZDParams;
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
            ((Process_EW_VM)DataContext).Process.Methods[method_ZD_ListBox.SelectedIndex] = method;
            ResourceManager.mainWindowVM.Tips = "更新了自定Method：" + method;

            // 更新完成后,要将临时参数列表拿掉,这样再向临时参数列表中添加/更新内容也不会影响刚刚添加的Method
            ((Process_EW_VM)DataContext).ZDParams = new ObservableCollection<Attribute>();
        }

        public void Delete_ZDMethod()
        {
            ListBox method_ZD_ListBox = ControlExtensions.FindControl<ListBox>(this, "method_ZD_ListBox");
            if (method_ZD_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要删除的Method！";
                return;
            }

            Method method = (Method)method_ZD_ListBox.SelectedItem;
            ((Process_EW_VM)DataContext).Process.Methods.Remove(method);
            ResourceManager.mainWindowVM.Tips = "已删除成员方法：" + method;
        }

        public void Add_CommParam()
        {
            ComboBox paramType_Comm_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "paramType_Comm_ComboBox");
            if (paramType_Comm_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定参数类型！";
                return;
            }

            TextBox paramName_Comm_TextBox = ControlExtensions.FindControl<TextBox>(this, "paramName_Comm_TextBox");
            if (paramName_Comm_TextBox.Text == null || paramName_Comm_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要提供参数名称！";
                return;
            }

            Attribute attribute = new Attribute((Type)paramType_Comm_ComboBox.SelectedItem, paramName_Comm_TextBox.Text);
            ((Process_EW_VM)DataContext).CommParams.Add(attribute);
            ResourceManager.mainWindowVM.Tips = "已在临时参数列表中添加参数：" + attribute;
        }

        public void Update_CommParam()
        {
            ListBox param_Comm_ListBox = ControlExtensions.FindControl<ListBox>(this, "param_Comm_ListBox");
            if (param_Comm_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要在临时参数列表中选定要修改的参数！";
                return;
            }

            ComboBox paramType_Comm_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "paramType_Comm_ComboBox");
            if (paramType_Comm_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定参数类型！";
                return;
            }

            TextBox paramName_Comm_TextBox = ControlExtensions.FindControl<TextBox>(this, "paramName_Comm_TextBox");
            if (paramName_Comm_TextBox.Text == null || paramName_Comm_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要提供参数名称！";
                return;
            }

            Attribute attribute = (Attribute)param_Comm_ListBox.SelectedItem;
            attribute.Type = (Type)paramType_Comm_ComboBox.SelectedItem;
            attribute.Identifier = paramName_Comm_TextBox.Text;
            ResourceManager.mainWindowVM.Tips = "已在临时参数列表中更新参数：" + attribute;
        }

        public void Delete_CommParam()
        {
            ListBox param_Comm_ListBox = ControlExtensions.FindControl<ListBox>(this, "param_Comm_ListBox");
            if (param_Comm_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要在临时参数列表中选定要删除的参数！";
                return;
            }

            Attribute attribute = (Attribute)param_Comm_ListBox.SelectedItem;
            ((Process_EW_VM)DataContext).CommParams.Remove(attribute);
            ResourceManager.mainWindowVM.Tips = "已在临时参数列表中删除参数：" + attribute;
        }

        public void Add_CommMethod()
        {
            ComboBox inout_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "inout_ComboBox");
            if (inout_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定CommMethod的输入/输出类型！";
                return;
            }

            TextBox commMethodName_TextBox = ControlExtensions.FindControl<TextBox>(this, "commMethodName_TextBox");
            if (commMethodName_TextBox.Text == null || commMethodName_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要给出CommMethod的方法名！";
                return;
            }

            ObservableCollection<Attribute> parameters = ((Process_EW_VM)DataContext).CommParams;
            if (parameters.Count == 0)
            {
                ResourceManager.mainWindowVM.Tips = "至少要在形参表中添加一个参数！";
                return;
            }

            CommMethod commMethod = new CommMethod(
                commMethodName_TextBox.Text,
                parameters,
                (InOut)inout_ComboBox.SelectedItem
            );

            ((Process_EW_VM)DataContext).Process.CommMethods.Add(commMethod);
            ResourceManager.mainWindowVM.Tips = "添加了CommMethod：" + commMethod;

            // 添加完成后,要将临时参数列表拿掉,这样再向临时参数列表中添加/更新内容也不会影响刚刚添加的CommMethod
            ((Process_EW_VM)DataContext).CommParams = new ObservableCollection<Attribute>();
        }

        public void Update_CommMethod()
        {
            ListBox commMethod_ListBox = ControlExtensions.FindControl<ListBox>(this, "commMethod_ListBox");
            if (commMethod_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要更新的CommMethod！";
                return;
            }

            ComboBox inout_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "inout_ComboBox");
            if (inout_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定CommMethod的输入/输出类型！";
                return;
            }

            TextBox commMethodName_TextBox = ControlExtensions.FindControl<TextBox>(this, "commMethodName_TextBox");
            if (commMethodName_TextBox.Text == null || commMethodName_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要给出CommMethod的方法名！";
                return;
            }

            ObservableCollection<Attribute> parameters = ((Process_EW_VM)DataContext).CommParams;
            if (parameters.Count == 0)
            {
                ResourceManager.mainWindowVM.Tips = "至少要在形参表中添加一个参数！";
                return;
            }

            CommMethod commMethod = new CommMethod(
                commMethodName_TextBox.Text,
                parameters,
                (InOut)inout_ComboBox.SelectedItem
            );

            ((Process_EW_VM)DataContext).Process.CommMethods[commMethod_ListBox.SelectedIndex] = commMethod;
            ResourceManager.mainWindowVM.Tips = "更新了CommMethod：" + commMethod;

            // 更新完成后,要将临时参数列表拿掉,这样再向临时参数列表中添加/更新内容也不会影响刚刚添加的CommMethod
            ((Process_EW_VM)DataContext).CommParams = new ObservableCollection<Attribute>();
        }

        public void Delete_CommMethod()
        {
            ListBox commMethod_ListBox = ControlExtensions.FindControl<ListBox>(this, "commMethod_ListBox");
            if (commMethod_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要删除的CommMethod！";
                return;
            }

            CommMethod commMethod = (CommMethod)commMethod_ListBox.SelectedItem;
            ((Process_EW_VM)DataContext).Process.CommMethods.Remove(commMethod);
            ResourceManager.mainWindowVM.Tips = "已删除CommMethod：" + commMethod;
        }

        #endregion

        #region 事件

        // 自定Method右侧列表选中项变化的处理
        private void method_ZD_ListBox_Changed(object sender, SelectionChangedEventArgs e)
        {
            // 将右侧选中项的参数列表拷贝到param_ZD_ListBox绑定的ZDParams里
            ListBox method_ZD_ListBox = ControlExtensions.FindControl<ListBox>(this, "method_ZD_ListBox");
            ((Process_EW_VM)DataContext).ZDParams = new ObservableCollection<Attribute>();
            foreach (Attribute attribute in ((Method)method_ZD_ListBox.SelectedItem).Parameters)
            {
                ((Process_EW_VM)DataContext).ZDParams.Add(new Attribute(attribute.Type, attribute.Identifier));
            }
            ResourceManager.mainWindowVM.Tips = "选中了Method：" + (Method)method_ZD_ListBox.SelectedItem + "，已拷贝其参数列表";
        }

        // CommMethod右侧列表选中项变化的处理
        private void commMethod_ListBox_Changed(object sender, SelectionChangedEventArgs e)
        {
            // 将右侧选中项的参数列表拷贝到param_Comm_ListBox绑定的CommParams里
            ListBox commMethod_ListBox = ControlExtensions.FindControl<ListBox>(this, "commMethod_ListBox");
            ((Process_EW_VM)DataContext).CommParams = new ObservableCollection<Attribute>();
            foreach (Attribute attribute in ((CommMethod)commMethod_ListBox.SelectedItem).Parameters)
            {
                ((Process_EW_VM)DataContext).CommParams.Add(new Attribute(attribute.Type, attribute.Identifier));
            }
            ResourceManager.mainWindowVM.Tips = "选中了CommMethod：" + (CommMethod)commMethod_ListBox.SelectedItem + "，已拷贝其参数列表";
        }

        #endregion

        #region 私有

        // 初始化.cs文件中的数据绑定,一些不方便在xaml中绑定的部分在这里绑定
        private void init_binding()
        {
            // 绑定Method的内置方法
            ListBox innerMethod_ListBox = ControlExtensions.FindControl<ListBox>(this, "innerMethod_ListBox");
            innerMethod_ListBox.Items = Method.InnerMethods;

            // 绑定Method的加密算法枚举
            ComboBox crypto_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "crypto_ComboBox");
            crypto_ComboBox.Items = System.Enum.GetValues(typeof(Crypto));
            crypto_ComboBox.SelectedItem = Crypto.None;

            // 绑定CommMethod的InOut枚举
            ComboBox inout_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "inout_ComboBox");
            inout_ComboBox.Items = System.Enum.GetValues(typeof(InOut));
            inout_ComboBox.SelectedItem = InOut.In;
        }

        // 初始化.cs文件中的事件处理方法,一些无法在xaml中绑定的部分在这里绑定
        private void init_event()
        {
            // 绑定自定Method右侧列表选中项变化的处理方法
            ListBox method_ZD_ListBox = ControlExtensions.FindControl<ListBox>(this, "method_ZD_ListBox");
            method_ZD_ListBox.SelectionChanged += method_ZD_ListBox_Changed;

            // 绑定CommMethod右侧列表选中项变化的处理方法
            ListBox commMethod_ListBox = ControlExtensions.FindControl<ListBox>(this, "commMethod_ListBox");
            commMethod_ListBox.SelectionChanged += commMethod_ListBox_Changed;
        }

        #endregion
    }
}
