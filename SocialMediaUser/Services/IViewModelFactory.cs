using SocialMediaUser.ViewModels;

namespace SocialMediaUser.Services;

public interface IViewModelFactory<TBase>
{
    public TBase Create<TViewModel>() where TViewModel : TBase;
}