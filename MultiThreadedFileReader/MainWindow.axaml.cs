using Avalonia;
using Avalonia.Controls;
using FluentAvalonia.Styling;

namespace MultiThreadedFileReader
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var thm = AvaloniaLocator.Current.GetService<FluentAvaloniaTheme>();
            thm?.ForceWin32WindowToTheme(this);
        }
    }
}