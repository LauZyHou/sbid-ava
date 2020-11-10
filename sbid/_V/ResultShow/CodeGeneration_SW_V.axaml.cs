using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using sbid._M;

namespace sbid._V
{
    public class CodeGeneration_SW_V : Window
    {
        public CodeGeneration_SW_V()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            this.get_control_reference();
            this.init_bindings();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #region 按钮命令

        // 代码生成：生成
        public void CodeGen_gen()
        {
            ResourceManager.mainWindowVM.Tips = "生成中...";
            // 这里拼上选中的编程语言类型
            string command_file = ResourceManager.CodeGen_gen + "_" + language_ComboBox.SelectedItem;
            bool res = Utils.runCommand
                (
                    command_file,
                    null
                );
            if (res)
            {
                ResourceManager.mainWindowVM.Tips = "运行了脚本：" + command_file;
            }
        }

        // 代码生成：生成、编译、执行
        public void CodeGen_gen_comp_run()
        {
            ResourceManager.mainWindowVM.Tips = "生成、编译、执行中...";
            // 这里拼上选中的编程语言类型
            string command_file = ResourceManager.CodeGen_gen_comp_run + "_" + language_ComboBox.SelectedItem;
            bool res = Utils.runCommand
                (
                    command_file,
                    null
                );
            if (res)
            {
                ResourceManager.mainWindowVM.Tips = "运行了脚本：" + command_file;
            }
        }

        #endregion

        #region 私有

        private ComboBox language_ComboBox;

        // 获取控件引用
        private void get_control_reference()
        {
            language_ComboBox = ControlExtensions.FindControl<ComboBox>(this, nameof(language_ComboBox));
        }

        // 为控件添加绑定
        private void init_bindings()
        {
            language_ComboBox.Items = System.Enum.GetValues(typeof(ProgramLanguage));
            language_ComboBox.SelectedItem = ProgramLanguage.C;
        }

        #endregion
    }
}
