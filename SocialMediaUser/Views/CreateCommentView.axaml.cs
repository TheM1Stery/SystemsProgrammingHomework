using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SocialMediaUser.Views;

public partial class CreateCommentView : UserControl
{
    public CreateCommentView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}