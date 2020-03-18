using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using sbid._M;
using sbid._VM;
using System.Collections.Generic;

namespace sbid._V
{
    public class SequenceDiagram_P_V : UserControl
    {
        public SequenceDiagram_P_V()
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
            // 绑定SeqMessage的InOut枚举
            ComboBox seqMessage_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "seqMessage_ComboBox");
            seqMessage_ComboBox.Items = System.Enum.GetValues(typeof(SeqMessage));
            // 绑定ConnectorVisible
            ComboBox connectorVisible_ComboBox = ControlExtensions.FindControl<ComboBox>(this, "connectorVisible_ComboBox");
            List<bool> boolList = new List<bool>();
            boolList.Add(true);
            boolList.Add(false);
            connectorVisible_ComboBox.Items = boolList;
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

        // 创建对象-生命线
        public void CreateObjLifeLineVM()
        {
            ObjLifeLine_VM objLifeLineVM = new ObjLifeLine_VM(mousePos.X, mousePos.Y);
            SequenceDiagramPVM.UserControlVMs.Add(objLifeLineVM);
            ResourceManager.mainWindowVM.Tips = "添加了对象-生命线";
        }

        #endregion

        // 对应的VM
        public SequenceDiagram_P_VM SequenceDiagramPVM { get => (SequenceDiagram_P_VM)DataContext; }
    }
}
