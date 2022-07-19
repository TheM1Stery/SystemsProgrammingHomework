using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using SimpleInjector;
using SocialMediaUser.Services;
using SocialMediaUser.ViewModels;
using SocialMediaUser.Views;

namespace SocialMediaUser
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
            container.Register<NavigationStore>(Lifestyle.Singleton);
            container.RegisterSingleton<IViewModelFactory>(() =>
            {
                var factory = new ViewModelFactory(new Dictionary<Type, Func<BaseViewModel>>()
                {
                    
                });
                return factory;
            });
            container.Register<INavigationService, NavigationService>(Lifestyle.Singleton);
            container.Register<MainView>(Lifestyle.Singleton);
            container.Register<MainViewModel>(Lifestyle.Singleton);
            container.Register<GoodbyeWorldViewModel>(Lifestyle.Singleton);
            container.Register<HelloWorldViewModel>(Lifestyle.Singleton);
            container.RegisterInitializer<MainView>(x =>
            {
                x.DataContext = container.GetInstance<MainViewModel>();
            });
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = container.GetInstance<MainView>();
            }
            base.OnFrameworkInitializationCompleted();
        }
    }
}