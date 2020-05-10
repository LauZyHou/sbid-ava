using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using sbid._M;
using sbid._VM;

namespace sbid._V
{
    public class TopoNode_EW_V : Window
    {
        public TopoNode_EW_V()
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

        #region 事件

        // 进程模板选中项变化的处理
        private void process_ComboBox_Changed(object sender, SelectionChangedEventArgs e)
        {
            // 安全锁锁定时不做任何修改
            if (TopoNodeEWVM.SafetyLock)
            {
                return;
            }
            // 清空属性列表
            TopoNodeEWVM.TopoNode.Properties.Clear();
            // 获取选中的Process
            ComboBox process_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "process_ComboBox");
            if (process_ComboBox.SelectedItem == null)
            {
                return;
            }
            Process process = (Process)process_ComboBox.SelectedItem;
            // 构造这个属性列表
            foreach (Attribute attribute in process.Attributes)
            {
                Instance instance;
                if (attribute.IsArray) // 数组
                {
                    instance = new ArrayInstance(attribute);
                }
                else if (attribute.Type is UserType) // 引用类型
                {
                    instance = ReferenceInstance.build(attribute);
                }
                else // 值类型
                {
                    instance = new ValueInstance(attribute);
                }
                TopoNodeEWVM.TopoNode.Properties.Add(instance);
            }
            ResourceManager.mainWindowVM.Tips = "进程模板被修改为[" + process.Name + "]，例化对象已重新生成";
        }

        #endregion

        #region 初始化

        // 初始化.cs文件中的数据绑定,一些不方便在xaml中绑定的部分在这里绑定
        private void init_binding()
        {
            // 绑定结点颜色枚举
            ComboBox color_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "color_ComboBox");
            color_ComboBox.Items = TopoNode.NodeColors;
        }

        // 初始化.cs文件中的事件处理方法,一些无法在xaml中绑定的部分在这里绑定
        private void init_event()
        {
            // 绑定进程模板ComboBox选中项变化的处理方法
            ComboBox process_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "process_ComboBox");
            process_ComboBox.SelectionChanged += process_ComboBox_Changed;
        }

        #endregion

        // 对应的VM
        public TopoNode_EW_VM TopoNodeEWVM { get => (TopoNode_EW_VM)DataContext; }
    }
}
