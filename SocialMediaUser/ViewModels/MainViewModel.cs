using CommunityToolkit.Mvvm.ComponentModel;
using SocialMediaUser.Services;

namespace SocialMediaUser.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    [ObservableProperty]
    private string? _title;
    
    public NavigationStore<BaseViewModel> Store { get; }

    public MainViewModel(NavigationStore<BaseViewModel> store, INavigationService<BaseViewModel> navigationService) 
        : base(navigationService)
    {
        Store = store;
        Title = "Social Media(User Panel)";
        Navigator.Navigate<LoginViewModel>();
    }
}