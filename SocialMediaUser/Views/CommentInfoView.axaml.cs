using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SocialMediaUser.Views;

public partial class CommentInfoView : UserControl
{
    public CommentInfoView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}