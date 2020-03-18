using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using sbid._VM;

namespace sbid._V
{
    public class AttackTree_P_V : UserControl
    {
        public AttackTree_P_V()
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

        // 创建攻击结点
        public void CreateAttackVM()
        {
            Attack_VM attackVM = new Attack_VM(mousePos.X, mousePos.Y);
            AttackTreePVM.UserControlVMs.Add(attackVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的攻击结点：" + attackVM.Attack.Content;
        }

        // 创建[与]关系
        public void CreateRelationVM_AND()
        {
            Relation_VM relationVM = new Relation_VM(mousePos.X, mousePos.Y) { Relation = _M.Relation.AND };
            AttackTreePVM.UserControlVMs.Add(relationVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的[与]关系结点(and)";
        }

        // 创建[或]关系
        public void CreateRelationVM_OR()
        {
            Relation_VM relationVM = new Relation_VM(mousePos.X, mousePos.Y) { Relation = _M.Relation.OR };
            AttackTreePVM.UserControlVMs.Add(relationVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的[或]关系结点(or)";
        }

        // 创建[非]关系
        public void CreateRelationVM_NEG()
        {
            Relation_VM relationVM = new Relation_VM(mousePos.X, mousePos.Y) { Relation = _M.Relation.NEG };
            AttackTreePVM.UserControlVMs.Add(relationVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的[非]关系结点(negation)";
        }

        // 创建[顺序与]关系
        public void CreateRelationVM_SAND()
        {
            Relation_VM relationVM = new Relation_VM(mousePos.X, mousePos.Y) { Relation = _M.Relation.SAND };
            AttackTreePVM.UserControlVMs.Add(relationVM);
            ResourceManager.mainWindowVM.Tips = "创建了新的[顺序与]关系结点(sequence and)";
        }

        #endregion

        // 对应的VM
        public AttackTree_P_VM AttackTreePVM { get => (AttackTree_P_VM)DataContext; }
    }
}
