using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CustomerDb.Views;

public partial class EditCustomerView : UserControl
{
    public EditCustomerView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}