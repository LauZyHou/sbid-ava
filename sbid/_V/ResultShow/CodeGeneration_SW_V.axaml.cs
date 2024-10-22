﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using sbid._M;
using sbid._VM;
using System.Runtime.InteropServices;

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
            // 这里拼上 "_" + 选中的编程语言类型 + "_" + 选中的代码执行平台
            string command_file = ResourceManager.CodeGen_gen + "_" + language_ComboBox.SelectedItem + "_" + platform_ComboBox.SelectedItem;
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

        // 代码生成：编译
        public void CodeGen_comp()
        {
            // 判断一下当前平台是不是和选中的平台是冲突的，如果是冲突的，没办法编译
            bool platformConflict = false;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) // 当前运行在Windows
            {
                platformConflict = platform_ComboBox.SelectedItem is ProgramPlatform.Unix;
            }
            else // 当前运行在Linux / OSX
            {
                platformConflict = platform_ComboBox.SelectedItem is ProgramPlatform.Windows;
            }
            if (platformConflict)
            {
                ResourceManager.mainWindowVM.Tips = "无效的操作。程序当前所在的操作系统无法编译选中的目标平台的代码";
                return;
            }

            // 这里拼上 "_" + 选中的编程语言类型 + "_" + 选中的代码执行平台
            string command_file = ResourceManager.CodeGen_comp + "_" + language_ComboBox.SelectedItem + "_" + platform_ComboBox.SelectedItem;
            string res = Utils.runCommandWithRes
                (
                    command_file,
                    null
                );
            if (res.Contains("error"))
            {
                ErrorBox_SW_V errorBox_SW_V = new ErrorBox_SW_V()
                {
                    DataContext = new ErrorBox_SW_VM()
                };
                errorBox_SW_V.ShowDialog(ResourceManager.mainWindowV);
                ((ErrorBox_SW_VM)errorBox_SW_V.DataContext).Content = "【编译不成功】" + res;
                ResourceManager.mainWindowVM.Tips = "编译不成功！";
            }
            else
            {
                ResourceManager.mainWindowVM.Tips = "启动了脚本：" + command_file + "，编译成功。";
            }
        }

        // 代码生成：运行
        public void CodeGen_run()
        {
            // 判断一下当前平台是不是和选中的平台是冲突的，如果是冲突的，没办法运行
            bool platformConflict = false;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) // 当前运行在Windows
            {
                platformConflict = platform_ComboBox.SelectedItem is ProgramPlatform.Unix;
            }
            else // 当前运行在Linux / OSX
            {
                platformConflict = platform_ComboBox.SelectedItem is ProgramPlatform.Windows;
            }
            if (platformConflict)
            {
                ResourceManager.mainWindowVM.Tips = "无效的操作。程序当前所在的操作系统无法运行选中的目标平台的代码";
                return;
            }

            // 这里拼上 "_" + 选中的编程语言类型 + "_" + 选中的代码执行平台
            string command_file = ResourceManager.CodeGen_run + "_" + language_ComboBox.SelectedItem + "_" + platform_ComboBox.SelectedItem;
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
            language_ComboBox.SelectedItem = ProgramLanguage.Cpp;
            platform_ComboBox.Items = System.Enum.GetValues(typeof(ProgramPlatform));
            platform_ComboBox.SelectedItem = ProgramPlatform.Unix;
        }

        #endregion
    }
}
