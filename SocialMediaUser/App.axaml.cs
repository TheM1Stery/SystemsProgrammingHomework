using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

namespace SocialMediaUser;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    // Make decorator a nested private class, so that there won't be any access to it outside the Composition root
    private partial class RepositoryDecorator<T> : IRepository<T> where T:class
    {
        private readonly Container _container;
        private readonly Func<IRepository<T>> _decorateeFactory;
        
        public RepositoryDecorator(Container container, Func<IRepository<T>> decorateeFactory)
        {
            _container = container;
            _decorateeFactory = decorateeFactory;
        }
    }
    
    public override void OnFrameworkInitializationCompleted()
    {
        var container = Bootstrap();
        // remove Avalonia validations, so that CommunityToolkitMVVM validations would work
        ExpressionObserver.DataValidators.RemoveAll(x => x is DataAnnotationsValidationPlugin);
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = container.GetInstance<MainView>();
        }
        base.OnFrameworkInitializationCompleted();
    }


    // Creates container
    private Container Bootstrap()
    {
        var container = new Container();
        container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
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
        container.Register<MainViewModel>(Lifestyle.Singleton);
        container.Register<MainView>(Lifestyle.Singleton);
        container.Register<LoginViewModel>();
        container.Register<RegisterViewModel>();
        container.Register<UserListViewModel>();
        container.Register<SocialMediaDbContext>(Lifestyle.Scoped);
        container.Register(typeof(IRepository<>), 
            typeof(SocialMediaRepository<>), Lifestyle.Scoped);
        container.Register<IHashCreatorService, HashCreatorService>(Lifestyle.Singleton);
        container.RegisterInitializer<MainView>(x =>
        {
            x.DataContext = container.GetInstance<MainViewModel>();
        });
        // this decorators will decorate IRepository implementations. This is needed so that scope behaviour could be added to
        // IRepository<> classes , otherwise SimpleInjector won't know when to dispose the IRepository<>.
        container.RegisterDecorator(typeof(IRepository<>), typeof(RepositoryDecorator<>),
            Lifestyle.Singleton);
        container.Verify();
        return container;
    }
}