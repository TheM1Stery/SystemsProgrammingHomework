using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CustomerDb.Views;

public partial class CustomerListView : UserControl
{
    public CustomerListView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}