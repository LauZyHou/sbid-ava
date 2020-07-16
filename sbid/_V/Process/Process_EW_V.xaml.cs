using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using sbid._M;
using sbid._VM;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace sbid._V
{
    public class Process_EW_V : Window
    {
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
            // 获取控件资源引用
            get_control_reference();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #region 辅助构造

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

            // 绑定CommMethod的CommWay枚举
            ComboBox commWay_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "commWay_ComboBox");
            commWay_ComboBox.Items = System.Enum.GetValues(typeof(CommWay));
            commWay_ComboBox.SelectedItem = CommWay.NativeEthernetFrame;
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

            // 绑定内置Method列表选中项变化的处理方法
            ListBox innerMethod_ListBox = ControlExtensions.FindControl<ListBox>(this, "innerMethod_ListBox");
            innerMethod_ListBox.SelectionChanged += innerMethod_ListBox_Changed;
        }

        #endregion

        #region 按钮命令

        public void Update_RefName()
        {
            if (refName_TextBox.Text == null || refName_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要给出进程模板名称！";
                return;
            }

            // 检查进程模板名称独一无二
            if(!ResourceManager.checkProcessName(VM.Process, refName_TextBox.Text))
            {
                ResourceManager.mainWindowVM.Tips = "不可用的名称，与其它进程模板重名！";
                return;
            }

            VM.Process.RefName.Content = refName_TextBox.Text;
            ResourceManager.mainWindowVM.Tips = "为进程模板设置了名称：" + VM.Process.RefName.Content;
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

            CheckBox attr_IsArray_CheckBox = ControlExtensions.FindControl<CheckBox>(this, nameof(attr_IsArray_CheckBox));
            if (attr_IsArray_CheckBox.IsChecked == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定是否是数组！";
                return;
            }

            if (!Checker.checkAttributeIdentifier(VM.Process.Attributes, null, attrId_TextBox.Text))
            {
                ResourceManager.mainWindowVM.Tips = "属性名重复！";
                return;
            }

            Attribute attribute = new Attribute(
                (sbid._M.Type)type_ListBox.SelectedItem,
                attrId_TextBox.Text,
                (bool)attr_IsArray_CheckBox.IsChecked
            );
            ((Process_EW_VM)DataContext).Process.Attributes.Add(attribute);
            ResourceManager.mainWindowVM.Tips = "为进程模板[" + ((Process_EW_VM)DataContext).Process.RefName + "]添加了成员变量：" + attribute;
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

            CheckBox attr_IsArray_CheckBox = ControlExtensions.FindControl<CheckBox>(this, nameof(attr_IsArray_CheckBox));
            if (attr_IsArray_CheckBox.IsChecked == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定是否是数组！";
                return;
            }

            // 获取要修改的Attribute对象
            Attribute attribute = ((Attribute)attr_ListBox.SelectedItem);

            if (!Checker.checkAttributeIdentifier(VM.Process.Attributes, attribute, attrId_TextBox.Text))
            {
                ResourceManager.mainWindowVM.Tips = "属性名重复！";
                return;
            }

            // 判断并删除Authenticity
            bool del_auth = attribute.Type is UserType ? JudgeAndDeleteAuthenticity((UserType)attribute.Type, (Type)type_ListBox.SelectedItem) : false;

            // todo 变量名判重

            // 在该Attribute对象上原地修改
            attribute.Type = (Type)type_ListBox.SelectedItem;
            attribute.Identifier = attrId_TextBox.Text;
            attribute.IsArray = (bool)attr_IsArray_CheckBox.IsChecked;
            ResourceManager.mainWindowVM.Tips = "为进程模板[" + ((Process_EW_VM)DataContext).Process.RefName + "]更新了成员变量：" + attribute + "。";

            // Tips补充
            if (del_auth)
            {
                ResourceManager.mainWindowVM.Tips += "[!]因UserType被修改，SecurityProperty中依赖于它的Authenticity的二级属性失配，被一同删除。";
            }
        }

        public void Delete_Attribute()
        {
            ListBox attr_ListBox = ControlExtensions.FindControl<ListBox>(this, "attr_ListBox");
            if (attr_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要删除的Attribute！";
                return;
            }

            // 获取要删除的Attribute对象
            Attribute attribute = (Attribute)attr_ListBox.SelectedItem;

            // 判断并删除Authenticity
            bool del_auth = attribute.Type is UserType ? JudgeAndDeleteAuthenticity((UserType)attribute.Type, null) : false;

            ((Process_EW_VM)DataContext).Process.Attributes.Remove(attribute);
            ResourceManager.mainWindowVM.Tips = "为进程模板[" + ((Process_EW_VM)DataContext).Process.RefName + "]删除了成员变量：" + attribute + "。";

            // Tips补充
            if (del_auth)
            {
                ResourceManager.mainWindowVM.Tips += "[!]因UserType被修改，SecurityProperty中依赖于它的Authenticity的二级属性失配，被一同删除。";
            }
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

            Method method_template = (Method)innerMethod_ListBox.SelectedItem;

            if(!Checker.checkMethodName(VM.Process.Methods, null, method_template.Name) || !Checker.checkCommMethodName(VM.Process.CommMethods, null, method_template.Name))
            {
                ResourceManager.mainWindowVM.Tips = "和已有的方法/通信方法重名！";
                return;
            }

            // 形参表要拷贝一份,以防止在自定Method中对其修改时影响到内置Method
            ObservableCollection<Attribute> paramerters = new ObservableCollection<Attribute>();
            foreach (Attribute attribute in method_template.Parameters)
            {
                paramerters.Add(new Attribute(attribute.Type, attribute.Identifier));
            }
            // 这里加密方法要用用户选中的,而不是内置方法模板里的None
            Method method = new Method(method_template.ReturnType, method_template.Name, paramerters, (Crypto)crypto_ComboBox.SelectedItem);
            ((Process_EW_VM)DataContext).Process.Methods.Add(method);
            ResourceManager.mainWindowVM.Tips = "为进程模板[" + ((Process_EW_VM)DataContext).Process.RefName + "]添加了成员方法：" + method;
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

            Method method_template = (Method)innerMethod_ListBox.SelectedItem;
            ObservableCollection<Attribute> paramerters = new ObservableCollection<Attribute>();
            foreach (Attribute attribute in method_template.Parameters)
            {
                paramerters.Add(new Attribute(attribute.Type, attribute.Identifier));
            }

            Method method = (Method)method_NZ_ListBox.SelectedItem;

            if (!Checker.checkMethodName(VM.Process.Methods, method, method_template.Name) || !Checker.checkCommMethodName(VM.Process.CommMethods, null, method_template.Name))
            {
                ResourceManager.mainWindowVM.Tips = "和已有的方法/通信方法重名！";
                return;
            }

            method.ReturnType = method_template.ReturnType;
            method.Name = method_template.Name;
            method.Parameters = paramerters;
            method.CryptoSuffix = (Crypto)crypto_ComboBox.SelectedItem;
            ResourceManager.mainWindowVM.Tips = "为进程模板[" + ((Process_EW_VM)DataContext).Process.RefName + "]更新了成员方法：" + method;
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

            if (param_ZD_IsArray_CheckBox.IsChecked == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定是否是数组！";
                return;
            }

            ListBox param_ZD_ListBox = ControlExtensions.FindControl<ListBox>(this, "param_ZD_ListBox");
            if (!Checker.checkAttributeIdentifier((ObservableCollection<Attribute>)param_ZD_ListBox.Items, null, paramName_ZD_TextBox.Text))
            {
                ResourceManager.mainWindowVM.Tips = "参数名重复！";
                return;
            }

            Attribute attribute = new Attribute(
                (Type)paramType_ZD_ComboBox.SelectedItem,
                paramName_ZD_TextBox.Text,
                (bool)param_ZD_IsArray_CheckBox.IsChecked
            );
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

            if (param_ZD_IsArray_CheckBox.IsChecked == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定是否是数组！";
                return;
            }

            Attribute attribute = (Attribute)param_ZD_ListBox.SelectedItem;

            if (!Checker.checkAttributeIdentifier((ObservableCollection<Attribute>)param_ZD_ListBox.Items, attribute, paramName_ZD_TextBox.Text))
            {
                ResourceManager.mainWindowVM.Tips = "参数名重复！";
                return;
            }

            attribute.Type = (Type)paramType_ZD_ComboBox.SelectedItem;
            attribute.Identifier = paramName_ZD_TextBox.Text;
            attribute.IsArray = (bool)param_ZD_IsArray_CheckBox.IsChecked;
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

            if (!Checker.checkMethodName(VM.Process.Methods, null, methodName_TextBox.Text) || !Checker.checkCommMethodName(VM.Process.CommMethods, null, methodName_TextBox.Text))
            {
                ResourceManager.mainWindowVM.Tips = "和已有的方法/通信方法重名！";
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

            Method method = (Method)method_ZD_ListBox.SelectedItem;

            if (!Checker.checkMethodName(VM.Process.Methods, method, methodName_TextBox.Text) || !Checker.checkCommMethodName(VM.Process.CommMethods, null, methodName_TextBox.Text))
            {
                ResourceManager.mainWindowVM.Tips = "和已有的方法/通信方法重名！";
                return;
            }

            method.ReturnType = (Type)returnType_ComboBox.SelectedItem;
            method.Name = methodName_TextBox.Text;
            method.Parameters = parameters;
            method.CryptoSuffix = Crypto.None;
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

            if (param_Comm_IsArray_CheckBox.IsChecked == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定是否是数组！";
                return;
            }

            ListBox param_Comm_ListBox = ControlExtensions.FindControl<ListBox>(this, "param_Comm_ListBox");
            if (!Checker.checkAttributeIdentifier((ObservableCollection<Attribute>)param_Comm_ListBox.Items, null, paramName_Comm_TextBox.Text))
            {
                ResourceManager.mainWindowVM.Tips = "参数名重复！";
                return;
            }

            Attribute attribute = new Attribute(
                (Type)paramType_Comm_ComboBox.SelectedItem,
                paramName_Comm_TextBox.Text,
                (bool)param_Comm_IsArray_CheckBox.IsChecked
            );
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

            if (param_Comm_IsArray_CheckBox.IsChecked == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定是否是数组！";
                return;
            }

            Attribute attribute = (Attribute)param_Comm_ListBox.SelectedItem;

            if (!Checker.checkAttributeIdentifier((ObservableCollection<Attribute>)param_Comm_ListBox.Items, attribute, paramName_Comm_TextBox.Text))
            {
                ResourceManager.mainWindowVM.Tips = "参数名重复！";
                return;
            }

            attribute.Type = (Type)paramType_Comm_ComboBox.SelectedItem;
            attribute.Identifier = paramName_Comm_TextBox.Text;
            attribute.IsArray = (bool)param_Comm_IsArray_CheckBox.IsChecked;
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
            ComboBox commWay_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "commWay_ComboBox");
            if (commWay_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定CommMethod的通信方式！";
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

            if (!Checker.checkMethodName(VM.Process.Methods, null, commMethodName_TextBox.Text) || !Checker.checkCommMethodName(VM.Process.CommMethods, null, commMethodName_TextBox.Text))
            {
                ResourceManager.mainWindowVM.Tips = "和已有的方法/通信方法重名！";
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
                (InOut)inout_ComboBox.SelectedItem,
                (CommWay)commWay_ComboBox.SelectedItem
            );

            ((Process_EW_VM)DataContext).Process.CommMethods.Add(commMethod);
            ResourceManager.mainWindowVM.Tips = "添加了CommMethod：" + commMethod.ShowString;

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

            ComboBox commWay_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "commWay_ComboBox");
            if (commWay_ComboBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定CommMethod的通信方式！";
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

            CommMethod commMethod = (CommMethod)commMethod_ListBox.SelectedItem;

            if (!Checker.checkMethodName(VM.Process.Methods, null, commMethodName_TextBox.Text) || !Checker.checkCommMethodName(VM.Process.CommMethods, commMethod, commMethodName_TextBox.Text))
            {
                ResourceManager.mainWindowVM.Tips = "和已有的方法/通信方法重名！";
                return;
            }

            commMethod.Name = commMethodName_TextBox.Text;
            commMethod.Parameters = parameters;
            commMethod.InOutSuffix = (InOut)inout_ComboBox.SelectedItem;
            commMethod.CommWay = (CommWay)commWay_ComboBox.SelectedItem;
            ResourceManager.mainWindowVM.Tips = "更新了CommMethod：" + commMethod.ShowString;

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
            ResourceManager.mainWindowVM.Tips = "已删除CommMethod：" + commMethod.ShowString;
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
                ((Process_EW_VM)DataContext).ZDParams.Add(new Attribute(attribute));
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
                ((Process_EW_VM)DataContext).CommParams.Add(new Attribute(attribute));
            }
            ResourceManager.mainWindowVM.Tips = "选中了CommMethod：" + ((CommMethod)commMethod_ListBox.SelectedItem).ShowString + "，已拷贝其参数列表";
        }

        // 内置Method列表选中项变化的处理
        private void innerMethod_ListBox_Changed(object sender, SelectionChangedEventArgs e)
        {
            // 根据不同内置Method应允许不同的算法
            ListBox innerMethod_ListBox = ControlExtensions.FindControl<ListBox>(this, "innerMethod_ListBox");
            ComboBox crypto_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "crypto_ComboBox");
            if (innerMethod_ListBox.SelectedItem == null) // 变成没有选中时，恢复全列表
            {
                crypto_ComboBox.Items = System.Enum.GetValues(typeof(Crypto));
                crypto_ComboBox.SelectedItem = Crypto.None;
                return;
            }
            Method method = (Method)innerMethod_ListBox.SelectedItem;
            switch (method.Name)
            {
                // 对称加解密
                case "SymEnc":
                case "SymDec":
                    crypto_ComboBox.Items = Method.Sym;
                    crypto_ComboBox.SelectedItem = Method.Sym[0];
                    break;
                // 签名和验证
                case "Sign":
                case "Verify":
                    crypto_ComboBox.Items = Method.ASym.Union(Method.Hash).ToList();
                    crypto_ComboBox.SelectedItem = Method.ASym[0];
                    break;
                // 如果添加了其他的内置方法，要在这里加逻辑
                default:
                    break;
            }
        }

        #endregion

        #region 私有


        /// <summary>
        /// 判断并删除某些SecurityProperty的Authenticity
        /// 当用户修改/删除Process的某个Attribute的Type
        /// 如果该Type是UserType，且被修改成的新Type和它不一致
        /// 会导致SecurityProperty中依赖于该UserType的Authenticity失效(因为二级属性失配)
        /// 该方法用于查找这样的Authenticity，并将其从SecurityProperty.Authenticities列表中删除
        /// </summary>
        /// <param name="oldType">被修改或删除的UserType</param>
        /// <param name="newType">被修改成的新Type，若是删除则传入null</param>
        /// <returns>本方法内是否做了删除Authenticity的操作</returns>
        bool JudgeAndDeleteAuthenticity(UserType oldType, Type newType)
        {
            bool deleted = false; // 指示是否做了删除操作
            if (oldType != newType)
            {
                // 当前协议面板VM
                Protocol_VM protocolVM = ResourceManager.mainWindowVM.SelectedItem;
                // 其下的类图面板VM
                ClassDiagram_P_VM classDiagram_P_VM = (ClassDiagram_P_VM)protocolVM.PanelVMs[0].SidePanelVMs[0];
                // 遍历查找SecurityProperty的Authenticity
                foreach (ViewModelBase item in classDiagram_P_VM.UserControlVMs)
                {
                    if (item is SecurityProperty_VM)
                    {
                        // 这里维护一个要删除的Authenticity列表，遍历结束后再统一删除
                        List<Authenticity> authenticities = new List<Authenticity>();
                        // 遍历查询
                        SecurityProperty_VM vm = (SecurityProperty_VM)item;
                        foreach (Authenticity authenticity in vm.SecurityProperty.Authenticities)
                        {
                            if (authenticity.AttributeA.Type == oldType
                                || authenticity.AttributeB.Type == oldType)
                            {
                                authenticities.Add(authenticity); // 加到待删除列表里
                            }
                        }
                        // 统一删除
                        foreach (Authenticity authenticity in authenticities)
                        {
                            vm.SecurityProperty.Authenticities.Remove(authenticity);
                            deleted = true; // 记录做了删除
                        }
                    }
                }
            }
            return deleted;
        }

        #endregion

        #region 资源引用

        private TextBox refName_TextBox;
        private CheckBox attr_IsArray_CheckBox, param_ZD_IsArray_CheckBox, param_Comm_IsArray_CheckBox;

        // 获取控件引用
        private void get_control_reference()
        {
            refName_TextBox = ControlExtensions.FindControl<TextBox>(this, nameof(refName_TextBox));
            attr_IsArray_CheckBox = ControlExtensions.FindControl<CheckBox>(this, nameof(attr_IsArray_CheckBox));
            param_ZD_IsArray_CheckBox = ControlExtensions.FindControl<CheckBox>(this, nameof(param_ZD_IsArray_CheckBox));
            param_Comm_IsArray_CheckBox = ControlExtensions.FindControl<CheckBox>(this, nameof(param_Comm_IsArray_CheckBox));
            attr_IsArray_CheckBox.IsChecked = param_ZD_IsArray_CheckBox.IsChecked = param_Comm_IsArray_CheckBox.IsChecked = false;
        }

        public Process_EW_VM VM { get => (Process_EW_VM)DataContext; }

        #endregion
    }
}
