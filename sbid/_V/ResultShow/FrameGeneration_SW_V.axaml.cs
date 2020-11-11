using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using sbid._M;

namespace sbid._V
{
    public class FrameGeneration_SW_V : Window
    {
        public FrameGeneration_SW_V()
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

        // 代码框架生成
        public void FrameGen()
        {
            // 这里拼上 "_" + 选中的编程语言类型 + "_" + 选中的代码执行平台
            string command_file = ResourceManager.FrameGen_gen + "_" + language_ComboBox.SelectedItem + "_" + platform_ComboBox.SelectedItem; ;
            bool res = Utils.runCommand
                (
                    command_file,
                    null
                );
            if (res)
            {
                ResourceManager.mainWindowVM.Tips = "启动了脚本：" + command_file;
            }
        }

        #endregion

        #region 私有

        private ComboBox language_ComboBox, platform_ComboBox;

        // 获取控件引用
        private void get_control_reference()
        {
            language_ComboBox = ControlExtensions.FindControl<ComboBox>(this, nameof(language_ComboBox));
            platform_ComboBox = ControlExtensions.FindControl<ComboBox>(this, nameof(platform_ComboBox));
        }

        // 为控件添加绑定
        private void init_bindings()
        {
            language_ComboBox.Items = System.Enum.GetValues(typeof(ProgramLanguage));
            language_ComboBox.SelectedItem = ProgramLanguage.C;
            platform_ComboBox.Items = System.Enum.GetValues(typeof(ProgramPlatform));
            platform_ComboBox.SelectedItem = ProgramPlatform.Windows;
        }

        #endregion
    }
}
