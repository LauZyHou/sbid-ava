using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using sbid._VM;
using System;

namespace sbid._V
{
    public class StateTrans_V : NetworkItem_V
    {
        public StateTrans_V()
        {
            this.InitializeComponent();
            this.init_binding();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }


        #region 辅助构造

        private void init_binding()
        {
            // 在最外层的Grid形态变化时同时改变从NetworkItem_VM继承下来的宽高属性
            // 这样就能时刻从VM里直接获取宽高了
            Grid root_grid = ControlExtensions.FindControl<Grid>(this, "root_grid");
            var observable = root_grid.GetObservable(Grid.BoundsProperty);
            observable.Subscribe(value =>
            {
                if (VM != null)
                {
                    // 这里是判断下Height和Width有没有变，没变就是用户在拖拽移动
                    // 实际上不判断也不会出错，但是多算了几遍可能会引起性能变差
                    if (VM.H == value.Height && VM.W == value.Width)
                        return;
                    VM.H = value.Height; // root_grid.Bounds.Height
                    VM.W = value.Width;
                    // 刷新锚点位置
                    VM.FlushConnectorPos();
                }
            });
        }
        
        #endregion

        public StateTrans_VM VM { get => (StateTrans_VM)DataContext; }
    }
}
