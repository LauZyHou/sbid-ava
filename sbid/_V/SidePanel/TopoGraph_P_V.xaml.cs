using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using sbid._M;
using sbid._VM;
using System.Collections.Generic;

namespace sbid._V
{
    public class TopoGraph_P_V : UserControl
    {
        public TopoGraph_P_V()
        {
            this.InitializeComponent();
            init_binding();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #region 辅助构造

        // 初始化.cs文件中的数据绑定,一些不方便在xaml中绑定的部分在这里绑定
        private void init_binding()
        {
            // 绑定TopoNodeShape枚举
            ComboBox topoNodeShape_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "topoNodeShape_ComboBox");
            topoNodeShape_ComboBox.Items = System.Enum.GetValues(typeof(TopoNodeShape));
            // 绑定TopoLinkType枚举
            // ComboBox topoLinkType_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "topoLinkType_ComboBox");
            // topoLinkType_ComboBox.Items = System.Enum.GetValues(typeof(TopoLinkType));
        }

        #endregion

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

        // 创建拓扑结点
        public void CreateTopoNodeVM()
        {
            TopoNode_VM topoNode_VM;
            switch (TopoGraphPVM.TopoNodeShape)
            {
                case TopoNodeShape.Circle:
                    topoNode_VM = new TopoNode_Circle_VM(mousePos.X, mousePos.Y);
                    ResourceManager.mainWindowVM.Tips = "添加了拓扑结点(圆形)";
                    break;
                case TopoNodeShape.Square:
                    topoNode_VM = new TopoNode_Square_VM(mousePos.X, mousePos.Y);
                    ResourceManager.mainWindowVM.Tips = "添加了拓扑结点(正方形)";
                    break;
                default:
                    ResourceManager.mainWindowVM.Tips = "[ERROR]发生在TopoGraph_P_V.xaml.cs";
                    return;
            }
            TopoGraphPVM.UserControlVMs.Add(topoNode_VM);
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
        public TopoGraph_P_VM TopoGraphPVM { get => (TopoGraph_P_VM)DataContext; }
    }
}
