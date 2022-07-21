using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using SocialMediaUser.Services;

namespace SocialMediaUser.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    public LoginViewModel(INavigationService<BaseViewModel> navigation) : base(navigation)
    {
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