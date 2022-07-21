using CommunityToolkit.Mvvm.ComponentModel;
using SocialMediaUser.ViewModels;

namespace SocialMediaUser.Services;


public partial class NavigationStore<TBase> : ObservableObject
{
    [ObservableProperty]
    private TBase? _currentViewModel;
}