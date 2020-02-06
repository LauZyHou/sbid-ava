using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using sbid._VM;
using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._V
{
    public class NetworkItem_V : UserControl
    {
        // 是否正在按下状态
        private bool isPressed = false;
        // 按下位置坐标
        Point pressPoint;
        // 旧的NetworkItem_VM的位置坐标(原始图形左上角点的坐标)
        Point oldLocation;
        // 可视化树上的祖先容器组件,NetworkItem会在它上面移动
        IVisual parentIVisual = null;

        #region NetworkItem的拖拽

        // 鼠标按下
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            if (e.MouseButton != MouseButton.Left)
                return;

            // 所在面板无法在构造函数里求得
            if (parentIVisual == null)
                parentIVisual = this.Parent.Parent.Parent;

            isPressed = true;
            pressPoint = e.GetPosition(parentIVisual);
            oldLocation = new Point(NetworkItemVM.X, NetworkItemVM.Y);

            ResourceManager.mainWindowVM.Tips = "鼠标按下，记录图形位置：" + oldLocation;

            e.Handled = true;
        }

        // 鼠标移动
        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);

            if (isPressed)
            {
                Point pos = e.GetPosition(parentIVisual);
                NetworkItemVM.X = oldLocation.X + pos.X - pressPoint.X;
                NetworkItemVM.Y = oldLocation.Y + pos.Y - pressPoint.Y;

                // 对所有锚点也要作相同的移动
                // fixme
                if (NetworkItemVM.ConnectorVMs != null)
                {
                    foreach (Connector_VM connectorVM in NetworkItemVM.ConnectorVMs)
                    {
                        connectorVM.X = oldLocation.X + pos.X - pressPoint.X;
                        connectorVM.Y = oldLocation.Y + pos.Y - pressPoint.Y;
                    }
                }

                ResourceManager.mainWindowVM.Tips = "拖拽图形，图形当前位置：" + NetworkItemVM.X + "," + NetworkItemVM.Y;
            }

            e.Handled = true;
        }

        // 鼠标释放
        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            base.OnPointerReleased(e);

            isPressed = false;

            ResourceManager.mainWindowVM.Tips = "完成移动";

            e.Handled = true;
        }

        #endregion

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
