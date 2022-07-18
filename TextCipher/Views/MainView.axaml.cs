using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using FluentAvalonia.Styling;
using FluentAvalonia.UI.Controls;

namespace TextCipher.Views
{
    public partial class MainView : CoreWindow
    {
        public MainView()
        {
            InitializeComponent();
            this.AttachDevTools(); // for debugging purposes, press f12 to see it 
            var thm = AvaloniaLocator.Current.GetService<FluentAvaloniaTheme>();
            thm?.ForceWin32WindowToTheme(this); // this method is a lifesaver
        }
    }
}