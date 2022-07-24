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
            container.Options.DefaultScopedLifestyle = ScopedLifestyle.Flowing;
            container.Register<INavigationStore<BaseViewModel>, NavigationStore<BaseViewModel>>(Lifestyle.Singleton);
            container.RegisterSingleton<IViewModelFactory<BaseViewModel>>(() =>
            {
                var factory = new ViewModelFactory<BaseViewModel>(new Dictionary<Type, Func<BaseViewModel>>()
                {
                    [typeof(LoginViewModel)] = () =>
                    {
                        using var scope = AsyncScopedLifestyle.BeginScope(container);
                        return scope.GetInstance<LoginViewModel>();
                    },
                    [typeof(RegisterViewModel)] = () =>
                    {
                        using var scope = AsyncScopedLifestyle.BeginScope(container);
                        return scope.GetInstance<RegisterViewModel>();
                    },
                    [typeof(UserListViewModel)] = () =>
                    {
                        using var scope = AsyncScopedLifestyle.BeginScope(container);
                        return scope.GetInstance<UserListViewModel>();
                    }
                });
                return factory;
            });
            container.Register<INavigationService<BaseViewModel>, NavigationService<BaseViewModel>>(Lifestyle.Singleton);
            container.Register<MainViewModel>(Lifestyle.Singleton);
            container.Register<LoginViewModel>(Lifestyle.Scoped);
            container.Register<RegisterViewModel>(Lifestyle.Scoped);
            container.Register<UserListViewModel>(Lifestyle.Scoped);
            container.Register<SocialMediaDbContext>(Lifestyle.Scoped);
            container.Register(typeof(IRepository<>), 
                typeof(SocialMediaRepository<>), Lifestyle.Scoped);
            container.Register<IHashCreatorService, HashCreatorService>(Lifestyle.Singleton);

            // remove Avalonia validations, so that CommunityToolkitMVVM validations would work
            ExpressionObserver.DataValidators.RemoveAll(x => x is DataAnnotationsValidationPlugin);
            
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