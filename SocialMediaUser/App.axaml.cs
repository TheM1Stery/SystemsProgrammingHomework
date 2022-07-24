using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using SocialMediaUser.Models;
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
            container.Register<INavigationStore<BaseViewModel>, NavigationStore<BaseViewModel>>(Lifestyle.Singleton);
            container.RegisterSingleton<IViewModelFactory<BaseViewModel>>(() =>
            {
                var factory = new ViewModelFactory<BaseViewModel>(new Dictionary<Type, Func<BaseViewModel>>()
                {
                    [typeof(LoginViewModel)] = () => container.GetInstance<LoginViewModel>(),
                    [typeof(RegisterViewModel)] = () => container.GetInstance<RegisterViewModel>(),
                    [typeof(UserListViewModel)] = () => container.GetInstance<UserListViewModel>()
                });
                return factory;
            });
            container.Register<INavigationService<BaseViewModel>, NavigationService<BaseViewModel>>(Lifestyle.Singleton);
            container.Register<MainView>(Lifestyle.Singleton);
            container.Register<MainViewModel>(Lifestyle.Singleton);
            container.Register<LoginViewModel>();
            container.Register<RegisterViewModel>();
            container.Register<UserListViewModel>();
            container.Register(typeof(IRepository<>), 
                typeof(SocialMediaRepository<>), Lifestyle.Singleton);
            container.Register<IHashCreatorService, HashCreatorService>(Lifestyle.Singleton);
            container.RegisterInitializer<MainView>(x =>
            {
                x.DataContext = container.GetInstance<MainViewModel>();
            });
            
            // remove Avalonia validations, so that CommunityToolkitMVVM validations would work
            ExpressionObserver.DataValidators.RemoveAll(x => x is DataAnnotationsValidationPlugin);
            
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = container.GetInstance<MainView>();
            }
            base.OnFrameworkInitializationCompleted();
        }
    }
}