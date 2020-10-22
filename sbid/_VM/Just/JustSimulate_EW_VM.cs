using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class JustSimulate_EW_VM : ViewModelBase
    {
        public void OnGen()
        {
            Utils.runCommand
                (
                    ResourceManager.justSimuGenCommand_file,
                    ResourceManager.justSimuGenCommand_param
                );
        }

        public void OnCompile()
        {
            Utils.runCommand
                (
                    ResourceManager.justCompileCommand_file,
                    ResourceManager.justCompileCommand_param
                );
        }

        public void OnRun()
        {
            Utils.runCommand
                (
                    ResourceManager.justRunCommand_file,
                    ResourceManager.justRunCommand_param
                );
        }
    }
}
