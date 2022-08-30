using CommunityToolkit.Mvvm.Input;
using SocialMediaUser.Services;

namespace SocialMediaUser.ViewModels;

public partial class CreateCommentViewModel : BaseViewModel
{
    public CreateCommentViewModel(INavigationService<BaseViewModel> navigation) : base(navigation)
    {
    }

    [RelayCommand]
    private void GoBack()
    {
        
    }
}