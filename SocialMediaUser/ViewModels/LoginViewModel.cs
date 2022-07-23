using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using MVVMUtils;
using SocialMediaUser.Models;
using SocialMediaUser.Services;

namespace SocialMediaUser.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    private readonly IRepository<User> _userRepository;

    public LoginViewModel(IHashCreatorService hashCreator, IRepository<User> userRepository,
        INavigationService<BaseViewModel> navigation) : base(navigation)
    {
        _userRepository = userRepository;
    }


    [RelayCommand]
    private void NavigateToRegister()
    {
        Navigator.Navigate<RegisterViewModel>();
    }


    [RelayCommand]
    private void Login()
    {
        MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            SystemDecorations = SystemDecorations.BorderOnly,
            Icon = Icon.Stop,
            ContentMessage = "Not implemented yet"
        }).Show();
    }
}