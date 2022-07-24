using CommunityToolkit.Mvvm.Input;
using SocialMediaUser.Services;

namespace SocialMediaUser.ViewModels;

public partial class UserListViewModel : BaseViewModel
{
    public UserListViewModel(INavigationService<BaseViewModel> navigation) : base(navigation)
    {
    }


    [RelayCommand]
    private void Cancel()
    {
        Navigator.Navigate<LoginViewModel>();
    }
}