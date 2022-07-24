using System;
using System.Linq;
using System.Threading;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using SocialMediaUser.Models;
using SocialMediaUser.Services;

namespace SocialMediaUser.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    private readonly IHashCreatorService _hashCreator;
    private readonly IRepository<User> _userRepository;
    
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string? _email;
    
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string? _password;

    [ObservableProperty]
    private string? _errorMessage;

    [ObservableProperty]
    private bool _isErrorMessageEnabled;

    public LoginViewModel(IHashCreatorService hashCreator, IRepository<User> userRepository,
        INavigationService<BaseViewModel> navigation) : base(navigation)
    {
        _hashCreator = hashCreator;
        _userRepository = userRepository;
    }
    
    
    [RelayCommand]
    private void NavigateToRegister()
    {
        Navigator.Navigate<RegisterViewModel>();
    }
    
    [RelayCommand(CanExecute = nameof(CanLogin))]
    private void Login()
    {
        ThreadPool.QueueUserWorkItem(_ =>
        {
            var enumerable = _userRepository.Find(x => x.Email == Email);
            var users = enumerable.ToList();
            if (!users.Any())
            {
                IsErrorMessageEnabled = true;
                ErrorMessage = "This email is not registered. Please register";
                return;
            }
            var user = users.First();
            var computedHash = _hashCreator.ComputeHash(_hashCreator.ComputeHash(_password) + user.PasswordSalt);
            if (computedHash != user.PasswordHash)
            {
                IsErrorMessageEnabled = true;
                ErrorMessage = "Invalid password";
                return;
            }
            Navigator.Navigate<UserListViewModel>();
        });

    }

    private bool CanLogin()
    {
        return !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password);
    }
}