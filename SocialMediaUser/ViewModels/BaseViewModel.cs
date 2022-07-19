using CommunityToolkit.Mvvm.ComponentModel;
using SocialMediaUser.Services;

namespace SocialMediaUser.ViewModels;

public class BaseViewModel : ObservableObject
{
    protected readonly INavigationService Navigation;

    public BaseViewModel(INavigationService navigation)
    {
        Navigation = navigation;
    }
}