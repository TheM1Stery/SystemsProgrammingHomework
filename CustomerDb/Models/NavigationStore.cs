using CommunityToolkit.Mvvm.ComponentModel;
using CustomerDb.ViewModels;
using MVVMUtils;

namespace CustomerDb.Models;

public partial class NavigationStore : ObservableObject, INavigationStore<BaseViewModel>
{
    [ObservableProperty]
    private BaseViewModel? _currentViewModel;
}