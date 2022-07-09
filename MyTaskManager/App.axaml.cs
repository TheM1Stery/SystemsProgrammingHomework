using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MyTaskManager.Models;
using MyTaskManager.Services;
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

            container.Register<MainView>(Lifestyle.Singleton);
            container.RegisterInitializer<MainView>(x =>
            {
                var vm = container.GetInstance<MainViewModel>();
                vm.Close += x.Close;
                x.DataContext = vm;
            });
            container.Register<MainViewModel>(Lifestyle.Singleton);
            container.Register<IProcessHandlerService, ProcessHandlerService>(Lifestyle.Singleton);
            container.Register<IProcessChooser, ProcessChooser>(Lifestyle.Singleton);
            container.Register<IMessageBoxShower, MessageBoxShower>(Lifestyle.Singleton);
            
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = container.GetInstance<MainView>();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}