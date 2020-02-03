using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using sbid._VM;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._V
{
    public class NetworkItem_V : UserControl
    {
        // 是否正在按下
        private bool isPressed = false;
        // 按下位置坐标
        Point pressPoint;
        // 旧的NetworkItem_VM的X和Y坐标
        private double x, y;

        // 鼠标按下
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            ResourceManager.mainWindowVM.Tips = NetworkItemVM.X + "," + NetworkItemVM.Y;
            isPressed = true;
            pressPoint = e.GetPosition(this.Parent.Parent.Parent);
            x = NetworkItemVM.X;
            y = NetworkItemVM.Y;

            e.Handled = true;
        }

        // 鼠标移动
        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);

            if (isPressed)
            {
                Point pos = e.GetPosition(this.Parent.Parent.Parent);
                ResourceManager.mainWindowVM.Tips = "鼠标正在移动" + pos;
                NetworkItemVM.X = (int)(x + pos.X - pressPoint.X);
                NetworkItemVM.Y = (int)(y + pos.Y - pressPoint.Y);
            }

            e.Handled = true;
        }

        // 鼠标释放
        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            base.OnPointerReleased(e);

            ResourceManager.mainWindowVM.Tips = "鼠标释放了";
            isPressed = false;

            e.Handled = true;
        }

        // 获取DataContext里的VM
        public NetworkItem_VM NetworkItemVM 
        { 
            get
            {
                return (NetworkItem_VM)DataContext;
            }
        }
    }
}
