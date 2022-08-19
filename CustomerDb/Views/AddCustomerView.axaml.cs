using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CustomerDb.Views;

public partial class AddCustomerView : UserControl
{
    public AddCustomerView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}