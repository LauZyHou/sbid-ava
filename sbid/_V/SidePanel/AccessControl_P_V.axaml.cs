﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using sbid._VM;
using System;

namespace sbid._V
{
    public class AccessControl_P_V : UserControl
    {
        public AccessControl_P_V()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #region 监听鼠标位置用

        private Point mousePos;

        // 无法直接获取到鼠标位置，必须在鼠标相关事件回调方法里取得
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            // 特别注意，要取得的不是相对这个ClassDiagram_P_V的位置，而是相对于里面的内容控件
            ItemsControl panel = ControlExtensions.FindControl<ItemsControl>(this, "panel");
            // 右键在这个面板上按下时
            if (e.GetCurrentPoint(panel).Properties.PointerUpdateKind == PointerUpdateKind.RightButtonPressed)
            {
                // 更新位置
                mousePos = e.GetPosition(panel);
            }
        }

        #endregion

        #region 右键菜单命令

        // 创建普通状态结点
        public void CreateStateVM()
        {
            State_VM stateVM = new State_VM(mousePos.X, mousePos.Y);
            // 因为只是用于访问控制的，这里不检查重名了
            VM.UserControlVMs.Add(stateVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的状态结点：" + stateVM.State.Name;
        }

        // 创建终止状态结点
        public void CreateFinalStateVM()
        {
            FinalState_VM finalStateVM = new FinalState_VM(mousePos.X, mousePos.Y);
            VM.UserControlVMs.Add(finalStateVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的终止状态结点";
        }

        // 创建转移关系结点
        public void CreateStateTransVM()
        {
            StateTrans_VM stateTrans_VM = new StateTrans_VM(mousePos.X, mousePos.Y);
            VM.UserControlVMs.Add(stateTrans_VM);
            ResourceManager.mainWindowVM.Tips = "创建了新的状态转移结点";
        }

        #endregion

        #region 按钮命令

        // 导出图片
        public async void ExportImage()
        {
            string path = await ResourceManager.GetSaveFileName("png");
            // 由于改用了ListBox这里要取用ListBox
            ListBox panel = ControlExtensions.FindControl<ListBox>(this, "panel");
            ResourceManager.RenderImage(path, panel);
        }

        #endregion

        // 对应的VM
        public AccessControl_P_VM VM { get => (AccessControl_P_VM)DataContext; }
    }
}