using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using sbid._VM;

namespace sbid._V
{
    public class AttackWithRelation_V : NetworkItem_V
    {
        public AttackWithRelation_V()
        {
            this.InitializeComponent();
            init_event();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #region 事件

        // 解锁:反转DataContext中的IsLock
        private void reverseIsLock(object sender, RoutedEventArgs e)
        {
            VM.IsLocked = !VM.IsLocked;
            ResourceManager.mainWindowVM.Tips = VM.IsLocked ? "锁定结点（将直接使用当前值，不再通过子结点计算）" : "解锁结点（其取值将由子结点计算）";
        }

        #endregion

        #region 初始化

        private void init_event()
        {
            // 锁头按下时反转IsLock属性
            Lock lockControl = ControlExtensions.FindControl<Lock>(this, nameof(lockControl));
            lockControl.Tapped += reverseIsLock;
        }

        #endregion

        public AttackWithRelation_VM VM { get=>(AttackWithRelation_VM)DataContext; }
    }
}
