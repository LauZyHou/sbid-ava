using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;

namespace sbid._V
{
    public class Preference_EW_V : Window
    {
        public Preference_EW_V()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            this.get_control_reference();
            this.init_event();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public double TextBoxMaxWidth
        {
            get
            {
                return Bounds.Width * 0.6;
            }
        }

        #region 事件处理

        private void init_event()
        {
            // 窗体大小变化时修改TextBox的MaxWidth
            // 这是为了防止出现文件PATH太长导致TextBox无限延长
            var observe = this.GetObservable(Window.BoundsProperty);
            observe.Subscribe(value =>
            {
                if (value.Width > 200)
                    this.project_TextBox.MaxWidth =
                    this.proVerif_TextBox.MaxWidth =
                    this.beagle_TextBox.MaxWidth =
                    value.Width - 200;
            });
        }

        #endregion


        #region 资源引用

        private TextBox project_TextBox, proVerif_TextBox, beagle_TextBox;

        private void get_control_reference()
        {
            project_TextBox = ControlExtensions.FindControl<TextBox>(this, nameof(project_TextBox));
            proVerif_TextBox = ControlExtensions.FindControl<TextBox>(this, nameof(proVerif_TextBox));
            beagle_TextBox = ControlExtensions.FindControl<TextBox>(this, nameof(beagle_TextBox));
        }

        #endregion
    }
}
