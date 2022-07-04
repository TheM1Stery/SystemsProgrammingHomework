using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MyTaskManager.ViewModels;
using MyTaskManager.Views;
using SimpleInjector;

namespace MyTaskManager
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var container = new Container();

            container.Register<MainViewModel>(Lifestyle.Singleton);
            
            
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainView
                {
                    DataContext = container.GetInstance<MainViewModel>()
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}