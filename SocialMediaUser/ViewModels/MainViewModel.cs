using SocialMediaUser.Services;

namespace SocialMediaUser.ViewModels;

public class MainViewModel : BaseViewModel
{
    public NavigationStore Store { get; }

    public MainViewModel(NavigationStore store, INavigationService navigationService) : base(navigationService)
    {
        Store = store;
    }
}