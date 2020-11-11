﻿using ReactiveUI;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace sbid._VM
{
    public class JustVerify_EW_VM : ViewModelBase
    {
        private string verifyResult;

        public string VerifyResult { get => verifyResult; set => this.RaiseAndSetIfChanged(ref verifyResult, value); }

        #region 按钮命令

        // 按下【验证当前协议模型】按钮
        private void OnVerify()
        {
            string command_file = ResourceManager.Verify_verify;
            string command_param = null;
            // 检查一下验证命令是不是空
            if (string.IsNullOrEmpty(command_file))
            {
                ResourceManager.mainWindowVM.Tips = "验证命令为空，无法验证";
                return;
            }
            // 自动识别，根据操作系统的不同，调用不同的后缀名脚本
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                command_file += ".bat";
            }
            else
            {
                command_param = command_file + ".sh";
                command_file = "bash";
            }

            // 要执行的验证命令
            ProcessStartInfo processStartInfo = new ProcessStartInfo
                (
                    command_file,
                    command_param
                )
            {
                RedirectStandardOutput = true
            };
            // 执行这条命令，执行过程中可能抛掷异常，直接显示异常信息
            Process process = null;
            try
            {
                process = Process.Start(processStartInfo);
            }
            catch (Exception ex)
            {
                ResourceManager.mainWindowVM.Tips = ex.ToString();
            }
            if (process is null)
            {
                return;
            }
            // 读取并写入结果
            using (StreamReader streamReader = process.StandardOutput)
            {
                if (!streamReader.EndOfStream)
                {
                    VerifyResult = streamReader.ReadToEnd();
                }

                if (!process.HasExited)
                {
                    process.Kill();
                }
            }
            ResourceManager.mainWindowVM.Tips = "启动了脚本：" + command_file + " " + command_param;
        }

        #endregion
    }
}
