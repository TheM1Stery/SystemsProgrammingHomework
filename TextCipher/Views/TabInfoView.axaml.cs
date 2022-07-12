using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TextCipher.Views;

public partial class TabInfoView : UserControl
{
    public TabInfoView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}