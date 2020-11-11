using ReactiveUI;
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
            string command_file = ResourceManager.justVerifyCommmad_file;
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
                command_file += ".sh";
            }

            // 要执行的验证命令
            ProcessStartInfo processStartInfo = new ProcessStartInfo
                (
                    command_file,
                    ResourceManager.justVerifyCommmad_param
                )
            {
                RedirectStandardOutput = true
            };
            ResourceManager.mainWindowVM.Tips = "开始验证...";
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
            ResourceManager.mainWindowVM.Tips = "验证完成";
        }

        #endregion
    }
}
