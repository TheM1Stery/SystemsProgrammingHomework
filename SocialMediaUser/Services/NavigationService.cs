using SocialMediaUser.ViewModels;

namespace SocialMediaUser.Services;

public class NavigationService<TBaseViewModel> : INavigationService<TBaseViewModel>
{
    private readonly NavigationStore<TBaseViewModel> _store;
    private readonly IViewModelFactory<TBaseViewModel> _factory;

    public NavigationService(NavigationStore<TBaseViewModel> store, IViewModelFactory<TBaseViewModel> factory)
    {
        _store = store;
        _factory = factory;
    }

    public void Navigate<TViewModel>() where TViewModel : TBaseViewModel
    {
        _store.CurrentViewModel = _factory.Create<TViewModel>();
    }
}