using CommunityToolkit.Mvvm.ComponentModel;
using MVVMUtils;

namespace CustomerDb.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    public INavigationStore<BaseViewModel> Store { get; }

    [ObservableProperty]
    private string? _title;

    public MainViewModel(INavigationStore<BaseViewModel> store, INavigationService<BaseViewModel> navigationService) : base(navigationService)
    {
        Store = store;
        Title = "CustomerDB";
        NavigationService.Navigate<CustomerListViewModel>();
    }
}