using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using sbid._VM;
using sbid._V;

namespace sbid
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow_V
                {
                    DataContext = new MainWindow_VM(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
