using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SocialMediaUser.Models;
using SocialMediaUser.Services;

namespace SocialMediaUser.ViewModels;

public partial class UserListViewModel : BaseViewModel
{
    private readonly IRepository<User> _userRepository;

    private readonly object _usersLock = new();
    

    [ObservableProperty]
    private ObservableCollection<User>? _users;


    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SearchCommand))]
    private string? _searchString;


    private bool CanSearch()
    {
        return !string.IsNullOrWhiteSpace(_searchString);
    }
    
    
    public UserListViewModel(IRepository<User> userRepository,
        INavigationService<BaseViewModel> navigation) : base(navigation)
    {
        _userRepository = userRepository;
    }


    [RelayCommand(CanExecute = nameof(CanSearch))]
    private void Search()
    {
        ThreadPool.QueueUserWorkItem(_ =>
        {
            lock (_usersLock)
            {
                var enumerable = _userRepository.Find(x => x.FirstName!.Contains(_searchString!) ||
                                                           x.LastName!.Contains(_searchString!));
                Users = new ObservableCollection<User>(enumerable);
            }
        });
    }
    
    
    [RelayCommand]
    private void Cancel()
    {
        Navigator.Navigate<LoginViewModel>();
    }
}