using CommunityToolkit.Mvvm.ComponentModel;
using SocialMediaUser.ViewModels;

namespace SocialMediaUser.Services;


public partial class NavigationStore : ObservableObject
{
    [ObservableProperty]
    private BaseViewModel? _currentViewModel;
}