using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using SocialMediaUser.Models;
using SocialMediaUser.Services;

namespace SocialMediaUser.ViewModels;

public partial class UserListViewModel : BaseViewModel
{

    public ObservableCollection<User> Users { get; } = new();

    public UserListViewModel(INavigationService<BaseViewModel> navigation) : base(navigation)
    {
    }
    
    
    [RelayCommand]
    private void Cancel()
    {
        Navigator.Navigate<LoginViewModel>();
    }
}