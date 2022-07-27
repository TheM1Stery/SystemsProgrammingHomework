using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using SocialMediaUser.Models;
using SocialMediaUser.Services;

namespace SocialMediaUser.ViewModels;

public partial class UserPostWallViewModel : BaseViewModel, IRecipient<ValueChangedMessage<User>>
{
    private readonly IRepository<Comment> _commentRepository;

    private User? _user;

    [ObservableProperty]
    private string? _title;


    [ObservableProperty]
    private ObservableCollection<Comment>? _comments;


    public UserPostWallViewModel(IRepository<Comment> commentRepository,
        INavigationService<BaseViewModel> navigation) : base(navigation)
    {
        _commentRepository = commentRepository;
        WeakReferenceMessenger.Default.Register(this);
    }

    [RelayCommand]
    private void Cancel()
    {
        Navigator.Navigate<UserListViewModel>();
    }

    public void Receive(ValueChangedMessage<User> message)
    {
        _user = message.Value;
        Title = $"{_user.FullName}'s wall";
        Comments = new ObservableCollection<Comment>(_commentRepository.Find(
            x => x.UserId == _user.Id));
    }
}