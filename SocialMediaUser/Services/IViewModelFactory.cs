using SocialMediaUser.ViewModels;

namespace SocialMediaUser.Services;

public interface IViewModelFactory
{
    public BaseViewModel Create<TViewModel>() where TViewModel : BaseViewModel;
}