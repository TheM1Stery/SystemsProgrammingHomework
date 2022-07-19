using CommunityToolkit.Mvvm.ComponentModel;
using SocialMediaUser.Services;

namespace SocialMediaUser.ViewModels;

public abstract class BaseViewModel : ObservableObject
{
    // Navigator is responsible for navigating through viewModels
    protected readonly INavigationService Navigator;

    protected BaseViewModel(INavigationService navigation)
    {
        Navigator = navigation;
    }
}