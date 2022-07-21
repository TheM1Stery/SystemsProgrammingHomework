using CommunityToolkit.Mvvm.ComponentModel;
using SocialMediaUser.ViewModels;

namespace SocialMediaUser.Services;


public partial class NavigationStore<TBaseViewModel> : ObservableObject
{
    [ObservableProperty]
    private TBaseViewModel? _currentViewModel;
}