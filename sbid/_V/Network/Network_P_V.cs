using Avalonia.Controls;
using Avalonia.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._V
{
    // 可拖拽/移动/锚点连线的面板V基类
    public class Network_P_V : UserControl
    {
        // 是否正在按下
        private bool isPressed = false;

        // 鼠标按下
        /*
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            ResourceManager.mainWindowVM.Tips = "鼠标按下了";
            isPressed = true;

            e.Handled = true;
        }
        */

        // 鼠标移动
        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);

            if (isPressed)
                ResourceManager.mainWindowVM.Tips = "鼠标正在移动";

            e.Handled = true;
        }

        // 鼠标释放
        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            base.OnPointerReleased(e);

            //ResourceManager.mainWindowVM.Tips = "鼠标释放了";
            isPressed = false;

            e.Handled = true;
        }
    }
}
