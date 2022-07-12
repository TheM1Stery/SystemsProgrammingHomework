using CommunityToolkit.Mvvm.ComponentModel;

namespace TextCipher.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    [ObservableProperty]
    private string? _title;


    public MainViewModel()
    {
        Title = "Text Cipher";
    }
}