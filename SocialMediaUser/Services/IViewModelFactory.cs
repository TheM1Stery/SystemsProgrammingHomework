using SocialMediaUser.ViewModels;

namespace SocialMediaUser.Services;

public interface IViewModelFactory
{
    public BaseViewModel Create<T>() where T : BaseViewModel;
}