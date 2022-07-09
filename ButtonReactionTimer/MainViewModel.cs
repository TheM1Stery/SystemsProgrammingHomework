using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ButtonReactionTimer;

[ObservableObject]
public partial class MainViewModel
{
    [ObservableProperty]
    private int _count;
    
    [RelayCommand]
    private void ButtonPress()
    {
        
    }
}