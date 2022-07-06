using CommunityToolkit.Mvvm.ComponentModel;

namespace TextCipher.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    [ObservableProperty]
    private string? _name;

    public MainViewModel()
    {
        Name = "Salam brat";
    }
}