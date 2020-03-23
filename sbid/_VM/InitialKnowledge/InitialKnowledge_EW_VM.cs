using sbid._M;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace sbid._VM
{
    public class InitialKnowledge_EW_VM
    {
        private InitialKnowledge initialKnowledge;
        private ObservableCollection<Process> processes = new ObservableCollection<Process>();

        // 要编辑的InitialKnowledge
        public InitialKnowledge InitialKnowledge { get => initialKnowledge; set => initialKnowledge = value; }
        // 集成协议下的所有Process,以用于KnowledgePair的构建和修改
        public ObservableCollection<Process> Processes { get => processes; set => processes = value; }

        #region 按钮命令

        // 设置为全局，即不设置进程模板
        public void SetGlobal()
        {
            if (initialKnowledge.Process == null)
            {
                ResourceManager.mainWindowVM.Tips = "已经是全局的了，无需重复操作";
                return;
            }
            initialKnowledge.Process = null;
            ResourceManager.mainWindowVM.Tips = "已取消关联进程模板，当前InitialKnowledge被视为全局的";
        }

        #endregion

    }
}
