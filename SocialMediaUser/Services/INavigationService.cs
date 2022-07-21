using SocialMediaUser.ViewModels;

namespace SocialMediaUser.Services;

public interface INavigationService<in TBase>
{
    public void Navigate<TViewModel>() where TViewModel : TBase;
}