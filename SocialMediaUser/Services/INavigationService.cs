namespace SocialMediaUser.Services;

public interface INavigationService<in TBaseViewModel>
{
    public void Navigate<TViewModel>() where TViewModel : TBaseViewModel;
}