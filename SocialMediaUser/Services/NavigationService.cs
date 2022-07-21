using SocialMediaUser.ViewModels;

namespace SocialMediaUser.Services;

public class NavigationService<TBase> : INavigationService<TBase>
{
    private readonly NavigationStore<TBase> _store;
    private readonly IViewModelFactory<TBase> _factory;

    public NavigationService(NavigationStore<TBase> store, IViewModelFactory<TBase> factory)
    {
        _store = store;
        _factory = factory;
    }

    public void Navigate<TViewModel>() where TViewModel : TBase
    {
        _store.CurrentViewModel = _factory.Create<TViewModel>();
    }
}