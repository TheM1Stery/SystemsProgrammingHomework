using CommunityToolkit.Mvvm.ComponentModel;
using MVVMUtils;

namespace CustomerDb.ViewModels;

public abstract class BaseViewModel : ObservableObject
{
    protected INavigationService<BaseViewModel> NavigationService;

    public BaseViewModel(INavigationService<BaseViewModel> navigationService)
    {
        NavigationService = navigationService;
    }
}