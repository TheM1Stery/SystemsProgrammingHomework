using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SocialMediaUser.Views;

public partial class UserListView : UserControl
{
    public UserListView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}