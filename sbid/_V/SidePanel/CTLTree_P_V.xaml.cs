﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using sbid._VM;

namespace sbid._V
{
    public class CTLTree_P_V : UserControl
    {
        public CTLTree_P_V()
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

        // 创建原子命题
        public void CreateAtomPropositionVM()
        {
            AtomProposition_VM apVM = new AtomProposition_VM(mousePos.X, mousePos.Y);
            CTLTreePVM.UserControlVMs.Add(apVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的原子命题：" + apVM.AtomProposition.RefName.Content;
        }

        // 创建CTL关系
        public void CreateCTLRelationVM()
        {
            CTLRelation_VM ctlrVM = new CTLRelation_VM(mousePos.X, mousePos.Y);
            CTLTreePVM.UserControlVMs.Add(ctlrVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的CTL关系结点";
        }

        // 创建逻辑关系
        public void CreateLogicRelationVM()
        {
            LogicRelation_VM logicrVM = new LogicRelation_VM(mousePos.X, mousePos.Y);
            CTLTreePVM.UserControlVMs.Add(logicrVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的逻辑关系结点";
        }

        #endregion

        #region 按钮命令

        // 导出图片
        public async void ExportImage()
        {
            string path = await ResourceManager.GetSaveFileName("png");
            ItemsControl panel = ControlExtensions.FindControl<ItemsControl>(this, "panel");
            ResourceManager.RenderImage(path, panel);
        }

        #endregion

        // 对应的VM
        public CTLTree_P_VM CTLTreePVM { get => (CTLTree_P_VM)DataContext; }
    }
}
