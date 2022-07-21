using SocialMediaUser.Services;

namespace SocialMediaUser.ViewModels;

public class RegisterViewModel : BaseViewModel
{
    public RegisterViewModel(INavigationService<BaseViewModel> navigation) : base(navigation)
    {
    }
}