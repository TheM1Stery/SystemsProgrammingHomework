using CommunityToolkit.Mvvm.ComponentModel;
using SocialMediaUser.Services;

namespace SocialMediaUser.ViewModels;

public abstract class BaseViewModel : ObservableObject
{
    // Navigator is responsible for navigating through viewModels
    protected readonly INavigationService<BaseViewModel> Navigator;

    protected BaseViewModel(INavigationService<BaseViewModel> navigation)
    {
        Navigator = navigation;
    }
}