using CommunityToolkit.Mvvm.Input;
using MVVMUtils;
using SocialMediaUser.Models;
using SocialMediaUser.Services;

namespace SocialMediaUser.ViewModels;

public partial class RegisterViewModel : BaseViewModel
{

    public RegistrationModel Form { get; } = new();
    
    public RegisterViewModel(IHashCreatorService hashCreator, INavigationService<BaseViewModel> navigation) : base(navigation)
    {
    }
    
    [RelayCommand]
    private void Cancel()
    {
        Navigator.Navigate<LoginViewModel>();
    }
}