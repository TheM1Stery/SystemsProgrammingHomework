using CommunityToolkit.Mvvm.Input;
using SocialMediaUser.Services;

namespace SocialMediaUser.ViewModels;

public partial class RegisterViewModel : BaseViewModel
{
    public RegisterViewModel(INavigationService<BaseViewModel> navigation) : base(navigation)
    {
    }


    [RelayCommand]
    private void Cancel()
    {
        Navigator.Navigate<LoginViewModel>();
    }
}