﻿using System;
using System.Collections.Generic;
using System.Text;
using sbid._M;
using sbid._V;

namespace sbid._VM
{
    public class InitialKnowledge_VM : NetworkItem_VM
    {
        private static int _id = 1;
        private InitialKnowledge initialKnowledge;

        public InitialKnowledge_VM()
        {
            initialKnowledge = new InitialKnowledge("未命名" + _id);
            _id++;
        }

        public InitialKnowledge InitialKnowledge { get => initialKnowledge; set => initialKnowledge = value; }

        #region 右键菜单命令

        // 尝试打开当前InitialKnowledge_VM的编辑窗体
        public void EditInitialKnowledgeVM()
        {
            // 从主窗体打开编辑窗体,并在其DataContext中集成当前InitialKnowledge_VM里集成的InitialKnowledge对象,以能对其作修改
            InitialKnowledge_EW_V initialKnowledgeEWV = new InitialKnowledge_EW_V()
            {
                DataContext = new InitialKnowledge_EW_VM()
                {
                    InitialKnowledge = initialKnowledge
                }
            };
            // 将所有的Process传入,作为KnowledgePair去选用的参数
            foreach (NetworkItem_VM item in ResourceManager.mainWindowVM.SelectedItem.SelectedItem.SelectedItem.NetworkItemVMs)
            {
                if (item is Process_VM)
                {
                    ((InitialKnowledge_EW_VM)initialKnowledgeEWV.DataContext).Processes.Add(((Process_VM)item).Process);
                }
            }

            initialKnowledgeEWV.ShowDialog(ResourceManager.mainWindowV);
            ResourceManager.mainWindowVM.Tips = "打开了InitialKnowledge：" + initialKnowledge.Name + "的编辑窗体";
        }

        #endregion
    }
}
