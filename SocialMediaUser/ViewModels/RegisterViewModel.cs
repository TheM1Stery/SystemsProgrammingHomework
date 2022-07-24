using System;
using System.Text;
using System.Threading;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using SocialMediaUser.Models;
using SocialMediaUser.Services;
using MessageBox.Avalonia.Enums;

namespace SocialMediaUser.ViewModels;

public partial class RegisterViewModel : BaseViewModel
{
    private readonly IHashCreatorService _hashCreator;
    private readonly IRepository<User> _userRepository;

    public RegistrationModel Form { get; } = new();
    
    public RegisterViewModel(IHashCreatorService hashCreator, IRepository<User> userRepository
        ,INavigationService<BaseViewModel> navigation) : base(navigation)
    {
        _hashCreator = hashCreator;
        _userRepository = userRepository;
    }
    
    

    [RelayCommand]
    private void Register()
    {
        if (Form.Email is null && Form.Password is null && Form.FirstName is null && Form.LastName is null
            && Form.DateOfBirth is null)
        {
            return;
        }
        if (Form.HasErrors)
        {
            var builder = new StringBuilder();
            foreach (var validationResult in Form.GetErrors())
            {
                builder.Append(validationResult.ErrorMessage + "\n");
            }
            MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams()
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SystemDecorations = SystemDecorations.BorderOnly,
                Icon = Icon.Error,
                ContentMessage = builder.ToString()
            }).Show();
            return;
        }
        ThreadPool.QueueUserWorkItem(_ =>
        {
            var salt = _hashCreator.ComputeSalt(Form.FirstName!.Length);
            var hash = _hashCreator.ComputeHash(_hashCreator.ComputeHash(Form.Password) + salt);
            _userRepository.Add(new User
            {
                DateOfBirth = Form.DateOfBirth!.Value.DateTime,
                FirstName = Form.FirstName,
                LastName = Form.LastName,
                PasswordHash = hash,
                PasswordSalt = salt,
                Email = Form.Email
            });
        });
    }
    
    
    [RelayCommand]
    private void Cancel()
    {
        Navigator.Navigate<LoginViewModel>();
    }
}