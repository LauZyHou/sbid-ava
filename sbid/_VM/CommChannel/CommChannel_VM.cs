using sbid._M;
using sbid._V;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._VM
{
    public class CommChannel_VM : NetworkItem_VM
    {
        private CommChannel commChannel;

        public CommChannel_VM()
        {
            this.commChannel = new CommChannel();
        }

        public CommChannel CommChannel { get => commChannel; set => commChannel = value; }

        #region 右键菜单命令

        // 尝试打开当前CommChannel_VM的编辑窗体
        public void EditCommChannelVM()
        {
            // 从主窗体打开编辑窗体,并在其DataContext中集成当前CommChannel_VM里集成的CommChannel对象,以能对其作修改
            CommChannel_EW_V commMethodEWV = new CommChannel_EW_V()
            {
                DataContext = new CommChannel_EW_VM()
                {
                    CommChannel = commChannel
                }
            };
            // 将所有的Process也传入,作为CommMethodPair的可用类型
            foreach (ViewModelBase item in ResourceManager.mainWindowVM.SelectedItem.SelectedItem.SelectedItem.UserControlVMs)
            {
                if (item is Process_VM)
                {
                    ((CommChannel_EW_VM)commMethodEWV.DataContext).Processes.Add(((Process_VM)item).Process);
                }
            }
            commMethodEWV.ShowDialog(ResourceManager.mainWindowV);
            ResourceManager.mainWindowVM.Tips = "打开了通信通道定义：" + commChannel.Name + "的编辑窗体";
        }

        #endregion
    }
}
