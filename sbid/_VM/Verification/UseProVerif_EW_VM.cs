using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace sbid._VM
{
    public class UseProVerif_EW_VM : ViewModelBase
    {
        private string verifyResult;

        public string VerifyResult { get => verifyResult; set => this.RaiseAndSetIfChanged(ref verifyResult, value); }

        #region 按钮命令

        // 按下【验证】按钮
        private void OnVerify()
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo("proverif", Environment.CurrentDirectory.Replace('\\', '/') + "/Assets/proverif.pv") { RedirectStandardOutput = true };
            Process process = Process.Start(processStartInfo); // 这里可能抛掷异常
            if (process is null)
            {
                ResourceManager.mainWindowVM.Tips = "无法执行的命令：" + processStartInfo.ToString();
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
        }

        #endregion
    }
}
