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
            container.Register<NavigationStore<BaseViewModel>>(Lifestyle.Singleton);
            container.RegisterSingleton<IViewModelFactory<BaseViewModel>>(() =>
            {
                var factory = new ViewModelFactory<BaseViewModel>(new Dictionary<Type, Func<BaseViewModel>>()
                {
                    [typeof(LoginViewModel)] = () => container.GetInstance<LoginViewModel>(),
                    [typeof(RegisterViewModel)] = () => container.GetInstance<RegisterViewModel>()
                });
                return factory;
            });
            container.Register<INavigationService<BaseViewModel>, NavigationService<BaseViewModel>>(Lifestyle.Singleton);
            container.Register<MainView>(Lifestyle.Singleton);
            container.Register<MainViewModel>(Lifestyle.Singleton);
            container.Register<LoginViewModel>();
            container.Register<RegisterViewModel>();
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