using System;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using MessageBox.Avalonia;
using SampleAppLauncher.Services;
using SampleAppLauncher.ViewModels;
using SampleAppLauncher.Views;
using SimpleInjector;

namespace SampleAppLauncher
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
                Environment.Exit(1);

            var container = new Container();
            
            container.Register<MainViewModel>(Lifestyle.Singleton);
            container.Register<MainView>(Lifestyle.Singleton);
            container.RegisterInitializer<MainView>(x =>
            {
                var vm = container.GetInstance<MainViewModel>();
                x.DataContext = vm;
                if (x.DataContext is IWindowCloser wc)
                {
                    wc.Close += x.Close;
                }
            });
            container.Register<IPathSelector, PathSelector>(Lifestyle.Singleton);
            container.Register<IMessageBoxShower, MessageBoxShower>(Lifestyle.Singleton);
            container.Register<IProcessStarter, ProcessStarter>(Lifestyle.Singleton);
            
            // remove Avalonia validations, so that CommunityToolkit mvvm validations would work
            ExpressionObserver.DataValidators.RemoveAll(x => x is DataAnnotationsValidationPlugin);

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                desktop.MainWindow = container.GetInstance<MainView>();
            

            base.OnFrameworkInitializationCompleted();
        }
    }
}