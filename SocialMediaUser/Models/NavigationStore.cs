using CommunityToolkit.Mvvm.ComponentModel;
using MVVMUtils;

namespace SocialMediaUser.Models;

public partial class NavigationStore<TBaseViewModel> : ObservableObject , INavigationStore<TBaseViewModel>
{
    [ObservableProperty]
    private TBaseViewModel? _currentViewModel;
}