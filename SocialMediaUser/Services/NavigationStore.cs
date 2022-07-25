using CommunityToolkit.Mvvm.ComponentModel;
namespace SocialMediaUser.Services;

public partial class NavigationStore<TBaseViewModel> : ObservableObject , INavigationStore<TBaseViewModel>
{
    [ObservableProperty]
    private TBaseViewModel? _currentViewModel;
}