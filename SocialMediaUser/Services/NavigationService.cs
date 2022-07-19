using SocialMediaUser.ViewModels;

namespace SocialMediaUser.Services;

public class NavigationService : INavigationService
{
    private readonly NavigationStore _store;
    private readonly IViewModelFactory _factory;

    public NavigationService(NavigationStore store, IViewModelFactory factory)
    {
        _store = store;
        _factory = factory;
    }

    public void Navigate<T>() where T : BaseViewModel
    {
        _store.CurrentViewModel = _factory.Create<T>();
    }
}