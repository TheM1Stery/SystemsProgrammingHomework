using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using SocialMediaUser.Models;
using SocialMediaUser.Services;

namespace SocialMediaUser.ViewModels;

public partial class CommentInfoViewModel : BaseViewModel
{
    public Comment Comment { get; }
    
    
    public CommentInfoViewModel(INavigationService<BaseViewModel> navigation) : base(navigation)
    {
        Comment = StrongReferenceMessenger.Default.Send(new RequestMessage<Comment>());
    }
}