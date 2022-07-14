using CommunityToolkit.Mvvm.ComponentModel;

namespace TextCipher.ViewModels;

public partial class TabInfoViewModel : BaseViewModel
{

    [ObservableProperty]
    private string? _message;
    
    [ObservableProperty]
    private int _progress;
    
    public TabInfoViewModel(string message)
    {
        Message = message;
    }
}