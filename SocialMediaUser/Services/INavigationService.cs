using SocialMediaUser.ViewModels;

namespace SocialMediaUser.Services;

public interface INavigationService
{
    public void Navigate<T>() where T : BaseViewModel;
}