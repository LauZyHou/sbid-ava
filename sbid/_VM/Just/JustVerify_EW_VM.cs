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

        // 按下【验证功能安全性质】按钮
        public void VerifySafety()
        {
            VerifyResult = Utils.runCommandWithRes(ResourceManager.Verify_safety, null);
        }

        // 按下【验证信息安全性质】按钮
        public void VerifySecurity()
        {
            VerifyResult = Utils.runCommandWithRes(ResourceManager.Verify_security, null);
        }

        #endregion
    }
}
