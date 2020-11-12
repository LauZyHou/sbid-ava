using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using sbid._M;
using sbid._VM;
using System.ComponentModel;

namespace sbid._V
{
    public class AttackWithRelation_EW_V : Window
    {
        public AttackWithRelation_EW_V()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            this.get_control_reference();
            this.Closing += close_event;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #region 辅助构造

        // 关闭窗体
        private void close_event(object sender, CancelEventArgs e)
        {
            AttackTree_P_VM atkTree_P_VM = (AttackTree_P_VM)ResourceManager.mainWindowVM.SelectedItem.SelectedItem.SelectedItem;
            atkTree_P_VM.PanelEnabled = true;
        }

        #endregion

        #region 按钮命令

        private void Save()
        {
            VM.AttackWithRelation.Description = description_TextBox.Text;
            VM.AttackWithRelation.AttackRelation = (AttackRelation)attackRelation_ComboBox.SelectedItem;
            VM.AttackWithRelation.RelationVisible = (bool)visible_CheckBox.IsChecked;
            ResourceManager.mainWindowVM.Tips = "保存了对攻击树结点的修改";
        }

        #endregion

        #region 资源引用

        ComboBox attackRelation_ComboBox;
        TextBox description_TextBox;
        CheckBox visible_CheckBox;

        private void get_control_reference()
        {
            attackRelation_ComboBox = ControlExtensions.FindControl<ComboBox>(this, nameof(attackRelation_ComboBox));
            attackRelation_ComboBox.Items = System.Enum.GetValues(typeof(AttackRelation));
            description_TextBox = ControlExtensions.FindControl<TextBox>(this, nameof(description_TextBox));
            visible_CheckBox = ControlExtensions.FindControl<CheckBox>(this, nameof(visible_CheckBox));
        }

        public AttackWithRelation_EW_VM VM { get => (AttackWithRelation_EW_VM)DataContext; }

        #endregion
    }
}
