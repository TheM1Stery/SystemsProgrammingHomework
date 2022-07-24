namespace SocialMediaUser.Services;

public interface INavigationStore<TBaseViewModel>
{
    public TBaseViewModel? CurrentViewModel { get; set; }
}