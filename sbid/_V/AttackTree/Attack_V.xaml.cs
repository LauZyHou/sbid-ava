using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using sbid._VM;

namespace sbid._V
{
    public class Attack_V : NetworkItem_V
    {
        public Attack_V()
        {
            this.InitializeComponent();

            this.init_event();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #region 事件

        // 解锁:反转DataContext中的IsLock
        private void reverseIsLock(object sender, RoutedEventArgs e)
        {
            ((Attack_VM)DataContext).IsLocked = !((Attack_VM)DataContext).IsLocked;
            ResourceManager.mainWindowVM.Tips = ((Attack_VM)DataContext).IsLocked ? "锁定攻击结点(结点锁定后将直接使用当前值,不再通过子结点计算)" : "解锁攻击结点(结点解锁后其取值将由子结点计算)";
        }

        #endregion

        #region 初始化

        private void init_event()
        {
            // 锁头按下时反转IsLock属性
            Lock lockShape = ControlExtensions.FindControl<Lock>(this, "lockShape");
            lockShape.Tapped += reverseIsLock;
        }

        #endregion
    }
}
