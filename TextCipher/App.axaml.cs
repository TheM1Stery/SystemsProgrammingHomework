using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using SimpleInjector;
using TextCipher.Factories;
using TextCipher.Services;
using TextCipher.ViewModels;
using TextCipher.Views;

namespace TextCipher
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
            container.Register<MainViewModel>(Lifestyle.Singleton);
            
            // property injection
            container.RegisterInitializer<MainView>(x => x.DataContext = 
                container.GetInstance<MainViewModel>());
            
            container.Register<IFileSelector, FileSelector>(Lifestyle.Singleton);
            container.Register<ITextFileGetterService, TextFileGetterService>(Lifestyle.Singleton);
            container.Register<ITabFactory, TabFactory>(Lifestyle.Singleton);
            container.Register<IEncryptionService, CaesarCypherEncryptionService>(Lifestyle.Singleton);

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = container.GetInstance<MainView>();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}