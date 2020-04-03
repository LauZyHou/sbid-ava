using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using sbid._VM;

namespace sbid._V
{
    public class TopoGraph_P_V : UserControl
    {
        public TopoGraph_P_V()
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

        // 创建拓扑结点
        public void CreateTopoNodeVM()
        {
            TopoNode_VM topoNode_VM = new TopoNode_VM(mousePos.X, mousePos.Y);
            TopoGraphPVM.UserControlVMs.Add(topoNode_VM);
            ResourceManager.mainWindowVM.Tips = "添加了拓扑结点";
        }

        #endregion

        // 对应的VM
        public TopoGraph_P_VM TopoGraphPVM { get => (TopoGraph_P_VM)DataContext; }
    }
}
