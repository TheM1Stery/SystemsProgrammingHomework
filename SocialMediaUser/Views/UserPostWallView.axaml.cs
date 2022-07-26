using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SocialMediaUser.Views;

public partial class UserPostWallView : UserControl
{
    public UserPostWallView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}