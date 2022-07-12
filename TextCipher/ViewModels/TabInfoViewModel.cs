using CommunityToolkit.Mvvm.ComponentModel;

namespace TextCipher.ViewModels;

public partial class TabInfoViewModel : BaseTabInfoViewModel
{

    [ObservableProperty]
    private string? _message;
    
    public TabInfoViewModel(string message)
    {
        Message = message;
    }
}