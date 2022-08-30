using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using SocialMediaUser.Models;
using SocialMediaUser.Services;

namespace SocialMediaUser.ViewModels;

public partial class UserPostWallViewModel : BaseViewModel, IRecipient<RequestMessage<Comment>>
{
    private readonly IRepository<Comment> _commentRepository;

    private User? _user;

    [ObservableProperty]
    private string? _title;
    
    [ObservableProperty]
    private ObservableCollection<Comment>? _comments;

    [ObservableProperty]
    private Comment? _selectedComment;

    [ObservableProperty]
    private CommentInfoViewModel? _currentComment;


    public UserPostWallViewModel(IRepository<Comment> commentRepository,
        INavigationService<BaseViewModel> navigation) : base(navigation)
    {
        _commentRepository = commentRepository;
        _user = StrongReferenceMessenger.Default.Send(new RequestMessage<User>());
        Title = $"{_user.FullName}'s wall";
        Comments = new ObservableCollection<Comment>(_commentRepository.Find(
            x => x.UserId == _user.Id));
        StrongReferenceMessenger.Default.Register(this);
    }

    [RelayCommand]
    private void Cancel()
    {
        Navigator.Navigate<UserListViewModel>();
        StrongReferenceMessenger.Default.Unregister<RequestMessage<Comment>>(this);
    }

    public void Receive(RequestMessage<Comment> message)
    {
        if (_selectedComment != null) 
            message.Reply(_selectedComment);
    }

    [RelayCommand]
    private void ShowComment()
    {
        CurrentComment = new CommentInfoViewModel(Navigator);
    }
}