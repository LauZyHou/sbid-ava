using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;

namespace sbid.ExtraControls
{
    public class MessageBox : Window
    {
        // 需要补充无参构造
        public MessageBox()
        {

        }

        public MessageBox(string title, string text1)
        {
            this.InitializeComponent();
            this.Title = title;
            ((TextBlock)(this.Content)).Text = text1;
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
