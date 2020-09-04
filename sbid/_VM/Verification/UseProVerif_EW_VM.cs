using ReactiveUI;
using System;
using System.Diagnostics;
using System.IO;

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
            // ProVerif待验证的pv文件路径
            string proVerifFilePath = ResourceManager.RunPath + "/Assets/proverif.pv";
            // ProVerif可执行文件位置，如果没有就用"proverif"代替，即期望用户配置了环境变量
            string proVerifPath = "proverif";
            if (!string.IsNullOrEmpty(ResourceManager.proVerifPath))
            {
                proVerifPath = ResourceManager.proVerifPath;
            }
            // 要执行的验证命令
            ProcessStartInfo processStartInfo = new ProcessStartInfo(proVerifPath, proVerifFilePath) { RedirectStandardOutput = true };
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
