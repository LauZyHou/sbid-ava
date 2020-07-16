using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using sbid._VM;

namespace sbid._V
{
    public class State_EW_V : Window
    {
        public State_EW_V()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            get_control_reference();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #region 按钮命令

        public void Update_Name()
        {
            if (name_TextBox.Text == null || name_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要给出状态名称！";
                return;
            }

            // 检查状态名称独一无二
            ProcessToSM_P_VM processToSM_P_VM = (ProcessToSM_P_VM)ResourceManager.mainWindowVM.SelectedItem.SelectedItem.SelectedItem;
            StateMachine_P_VM stateMachine_P_VM = processToSM_P_VM.SelectedItem;
            if (!Checker.checkStateName(stateMachine_P_VM, VM.State, name_TextBox.Text))
            {
                ResourceManager.mainWindowVM.Tips = "不可用的名称，与其它状态重名！";
                return;
            }

            VM.State.Name = name_TextBox.Text;
            ResourceManager.mainWindowVM.Tips = "为状态设置了名称：" + VM.State.Name;
        }

        #endregion

        #region 资源引用

        private TextBox name_TextBox;

        private void get_control_reference()
        {
            name_TextBox = ControlExtensions.FindControl<TextBox>(this, nameof(name_TextBox));
        }

        public State_EW_VM VM { get => (State_EW_VM)DataContext; }

        #endregion
    }
}
