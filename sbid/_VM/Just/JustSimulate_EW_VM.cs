using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class JustSimulate_EW_VM : ViewModelBase
    {
        public void OnGen()
        {
            ResourceManager.mainWindowVM.Tips = "生成中...";
            bool res = Utils.runCommand
                (
                    ResourceManager.justSimuGenCommand_file,
                    ResourceManager.justSimuGenCommand_param
                );
            if (res)
            {
                ResourceManager.mainWindowVM.Tips = "生成完毕。";
            }
        }

        public void OnCompile()
        {
            ResourceManager.mainWindowVM.Tips = "编译中...";
            bool res = Utils.runCommand
                (
                    ResourceManager.justCompileCommand_file,
                    ResourceManager.justCompileCommand_param
                );
            if (res)
            {
                ResourceManager.mainWindowVM.Tips = "编译完毕。";
            }
        }

        public void OnRun()
        {
            ResourceManager.mainWindowVM.Tips = "模拟运行中...";
            bool res = Utils.runCommand
                (
                    ResourceManager.justRunCommand_file,
                    ResourceManager.justRunCommand_param
                );
            if (res)
            {
                ResourceManager.mainWindowVM.Tips = "模拟运行完毕。";
            }
        }
    }
}
