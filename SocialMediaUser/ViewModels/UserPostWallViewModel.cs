using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using SocialMediaUser.Models;
using SocialMediaUser.Services;

namespace SocialMediaUser.ViewModels;

public partial class UserPostWallViewModel : BaseViewModel, IRecipient<ValueChangedMessage<User>>
{

    private User? _user;
    
    public UserPostWallViewModel(INavigationService<BaseViewModel> navigation) : base(navigation)
    {
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
    }
}