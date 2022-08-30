using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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

    // Make proxy a nested private class, so that there won't be any access to it outside the Composition root
    private partial class RepositoryProxy<T> : IRepository<T> where T:class
    {
        private readonly Container _container;
        private readonly Func<IRepository<T>> _decorateeFactory;
        
        public RepositoryProxy(Container container, Func<IRepository<T>> decorateeFactory)
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

    private static Func<BaseViewModel> CreateProducer<T>(Container container, Lifestyle lifestyle)
        where T : BaseViewModel => lifestyle.CreateProducer<BaseViewModel, T>(container).GetInstance;

    // Creates container
    private Container Bootstrap()
    {
        var container = new Container();
        container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
        container.Options.EnableAutoVerification = false;
        container.Register<INavigationStore<BaseViewModel>, NavigationStore<BaseViewModel>>(Lifestyle.Singleton);
        container.Register<RegistrationModel>();
        container.RegisterSingleton<IViewModelFactory<BaseViewModel>>(() =>
        {
            var factory = new ViewModelFactory<BaseViewModel>(new Dictionary<Type, Func<BaseViewModel>>()
            {
                [typeof(LoginViewModel)] = CreateProducer<LoginViewModel>(container, Lifestyle.Transient),
                [typeof(RegisterViewModel)] =  CreateProducer<RegisterViewModel>(container, Lifestyle.Transient),
                [typeof(UserListViewModel)] = CreateProducer<UserListViewModel>(container, Lifestyle.Transient),
                [typeof(UserPostWallViewModel)] = CreateProducer<UserPostWallViewModel>(container, Lifestyle.Transient),
                [typeof(CreateCommentViewModel)] = CreateProducer<CreateCommentViewModel>(container, Lifestyle.Transient)
            });
            return factory;
        });
        container.Register<INavigationService<BaseViewModel>, NavigationService<BaseViewModel>>(Lifestyle.Singleton);
        container.Register<MainViewModel>(Lifestyle.Singleton);
        container.Register<MainView>(Lifestyle.Singleton);
        container.Register<SocialMediaDbContext>(Lifestyle.Scoped);
        container.Register(typeof(IRepository<>), 
            typeof(SocialMediaRepository<>), Lifestyle.Scoped);
        container.Register<IHashCreatorService, HashCreatorService>(Lifestyle.Singleton);
        container.RegisterInitializer<MainView>(x =>
        {
            x.DataContext = container.GetInstance<MainViewModel>();
        });
        container.RegisterDecorator(typeof(IRepository<>), typeof(RepositoryProxy<>),
            Lifestyle.Singleton);
        return container;
    }
}