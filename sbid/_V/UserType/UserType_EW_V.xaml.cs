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
            }

            // todo 变量名判重

            Attribute attribute = new Attribute((sbid._M.Type)type_ListBox.SelectedItem, attrId_TextBox.Text);
            ((UserType_EW_VM)DataContext).UserType.Attributes.Add(attribute);
            ResourceManager.mainWindowVM.Tips = "为自定义类型[" + ((UserType_EW_VM)DataContext).UserType.Name + "]添加了成员变量：" + attribute;
        }

        public void Update_Attribute()
        {
            ListBox attr_ListBox = ControlExtensions.FindControl<ListBox>(this, "attr_ListBox");
            if (attr_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要修改的Attribute！";
                return;
            }

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
            }

            // todo 变量名判重

            Attribute attribute = ((Attribute)attr_ListBox.SelectedItem);
            attribute.Type = (sbid._M.Type)type_ListBox.SelectedItem;
            attribute.Identifier = attrId_TextBox.Text;
            ResourceManager.mainWindowVM.Tips = "为自定义类型[" + ((UserType_EW_VM)DataContext).UserType.Name + "]更新了成员变量：" + attribute;
        }

        public void Delete_Attribute()
        {
            ListBox attr_ListBox = ControlExtensions.FindControl<ListBox>(this, "attr_ListBox");
            if (attr_ListBox.SelectedItem == null)
            {
                ResourceManager.mainWindowVM.Tips = "需要选定要删除的Attribute！";
                return;
            }

            Attribute attribute = (Attribute)attr_ListBox.SelectedItem;
            ((UserType_EW_VM)DataContext).UserType.Attributes.Remove(attribute);
            ResourceManager.mainWindowVM.Tips = "为自定义类型[" + ((UserType_EW_VM)DataContext).UserType.Name + "]删除了成员变量：" + attribute;
        }

        #endregion
    }
}
