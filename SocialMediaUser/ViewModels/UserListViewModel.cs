using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using SocialMediaUser.Models;
using SocialMediaUser.Services;

namespace SocialMediaUser.ViewModels;

public partial class UserListViewModel : BaseViewModel, IRecipient<RequestMessage<User>>
{
    private readonly IRepository<User> _userRepository;

    private readonly object _usersLock = new();
    

    [ObservableProperty]
    private ObservableCollection<User>? _users;


    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SearchCommand))]
    private string? _searchString;

    [ObservableProperty]
    private User? _selectedUser;


    private bool CanSearch()
    {
        return !string.IsNullOrWhiteSpace(_searchString);
    }
    
    
    public UserListViewModel(IRepository<User> userRepository,
        INavigationService<BaseViewModel> navigation) : base(navigation)
    {
        _userRepository = userRepository;
        StrongReferenceMessenger.Default.Register(this);
    }

    [RelayCommand]
    private void NavigateToUserWall()
    {
        if (_selectedUser is null)
            return;
        Navigator.Navigate<UserPostWallViewModel>();
        // WeakReferenceMessenger.Default.Send(new ValueChangedMessage<User>(_selectedUser));
    }


    [RelayCommand(CanExecute = nameof(CanSearch))]
    private void Search()
    {
        ThreadPool.QueueUserWorkItem(_ =>
        {
            var enumerable = _userRepository.Find(x => x.FirstName!.Contains(_searchString!) ||
                                                       x.LastName!.Contains(_searchString!));
            lock (_usersLock)
            {
                Users = new ObservableCollection<User>(enumerable);
            }
        });
    }
    
    
    [RelayCommand]
    private void Cancel()
    {
        Navigator.Navigate<LoginViewModel>();
    }

    public void Receive(RequestMessage<User> message)
    {
        if (_selectedUser != null) 
            message.Reply(_selectedUser);
        StrongReferenceMessenger.Default.Unregister<RequestMessage<User>>(this);
    }
}