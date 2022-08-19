using System;
using System.Net;
using System.Net.Http;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using CustomerDb.Models;
using CustomerDb.Services;
using CustomerDb.ViewModels;
using CustomerDb.Views;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;
using Microsoft.Extensions.Configuration;
using MVVMUtils;
using SimpleInjector;

namespace CustomerDb
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private readonly Container _container = new();


        private partial class ViewModelFactory<T> : IViewModelFactory<T>
        {
        }

        public override void OnFrameworkInitializationCompleted()
        {
            Bootstrap();
            // remove Avalonia validations, so that CommunityToolkitMVVM validations would work
            ExpressionObserver.DataValidators.RemoveAll(x => x is DataAnnotationsValidationPlugin);
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = _container.GetInstance<MainView>();
            }
            base.OnFrameworkInitializationCompleted();
        }


        private void Bootstrap()
        {
            _container.Options.AllowOverridingRegistrations = true;
            _container.Options.EnableAutoVerification = false;
            _container.RegisterSingleton<MainView>();
            _container.RegisterInitializer<MainView>(x =>
            {
                var vm = _container.GetInstance<MainViewModel>();
                vm.Close += x.Close;
                x.DataContext = vm;
            });
            _container.Register<INavigationService<BaseViewModel>, NavigationService<BaseViewModel>>(Lifestyle.Singleton);
            _container.Register<INavigationStore<BaseViewModel>, NavigationStore>(Lifestyle.Singleton);
            _container.Register<IViewModelFactory<BaseViewModel>, ViewModelFactory<BaseViewModel>>(Lifestyle.Singleton);
            foreach (var reg in _container.GetTypesToRegister<BaseViewModel>(typeof(BaseViewModel).Assembly))
            {
                _container.Register(reg);
            }
            _container.Register<MainViewModel>(Lifestyle.Singleton);
            var config = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json").Build();
            var connectionString = config.GetConnectionString("SqlConnection");
            _container.RegisterSingleton<ICustomerDbClient>(() => new CustomerDbClient(connectionString!));
            _container.RegisterSingleton<IModalMessageBox, ModalMessageBox>();
        }
    }
}