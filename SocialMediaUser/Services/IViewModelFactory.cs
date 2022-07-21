using SocialMediaUser.ViewModels;

namespace SocialMediaUser.Services;

public interface IViewModelFactory<TBaseViewModel>
{
    public TBaseViewModel Create<TViewModel>() where TViewModel : TBaseViewModel;
}