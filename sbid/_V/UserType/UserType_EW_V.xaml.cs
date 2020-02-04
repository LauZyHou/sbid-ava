using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using sbid._M;
using sbid._VM;

namespace sbid._V
{
    public class UserType_EW_V : Window
    {

        public UserType_EW_V()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #region 按钮命令

        public void Add_Attribute()
        {
            ListBox type_ListBox = ControlExtensions.FindControl<ListBox>(this, "type_ListBox");
            if (type_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定成员类型！";
                return;
            }

            TextBox attrId_TextBox = ControlExtensions.FindControl<TextBox>(this, "attrId_TextBox");
            if (attrId_TextBox.Text == null || attrId_TextBox.Text.Length == 0)
            {
                ResourceManager.mainWindowVM.Tips = "需要给出成员名称！";
                return;
                // todo 变量名判重
            }

            Attribute attribute = new Attribute((sbid._M.Type)type_ListBox.SelectedItem, attrId_TextBox.Text);
            ((UserType_EW_VM)DataContext).UserType.Attributes.Add(attribute);
            ResourceManager.mainWindowVM.Tips = "为自定义类型" + ((UserType_EW_VM)DataContext).UserType.Name + "添加了成员变量：" + attribute.Type.Name + " " + attribute.Identifier;
        }

        #endregion
    }
}
