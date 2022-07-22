using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SocialMediaUser.Models;
using SocialMediaUser.Services;

namespace SocialMediaUser.ViewModels;

public partial class RegisterViewModel : BaseViewModel
{

    public RegistrationModel Form { get; } = new();
    
    public RegisterViewModel(INavigationService<BaseViewModel> navigation) : base(navigation)
    {
    }


    [RelayCommand]
    private void Cancel()
    {
        Navigator.Navigate<LoginViewModel>();
    }
}