using CommunityToolkit.Mvvm.Input;
using SocialMediaUser.Services;

namespace SocialMediaUser.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    public LoginViewModel(INavigationService navigation) : base(navigation)
    {
    }


    [RelayCommand]
    private void NavigateToRegister()
    {
        Navigator.Navigate<RegisterViewModel>();
    }
}