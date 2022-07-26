using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using SocialMediaUser.Services;

namespace SocialMediaUser.Models;

public partial class RegistrationModel : ObservableValidator
{
    private readonly IRepository<User> _userRepo;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [MinLength(3)]
    [Required]
    [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Your name must only contain letter characters")]
    private string? _firstName;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [MinLength(3)]
    [Required]
    [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Your name must only contain letter characters")]
    private string? _lastName;

    [ObservableProperty]
    [Required]
    private DateTimeOffset? _dateOfBirth;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [EmailAddress]
    [CustomValidation(typeof(RegistrationModel), nameof(ValidateEmail))]
    [Required]
    private string? _email;
    
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [MinLength(5)]
    [Required]
    private string? _password;

    public RegistrationModel(IRepository<User> userRepo)
    {
        _userRepo = userRepo;
    }

    public static ValidationResult? ValidateEmail(string email, ValidationContext context)
    {
        var instance = (RegistrationModel)context.ObjectInstance;
        var emailExists = instance._userRepo.Find(x => x.Email == email).Any();
        return !emailExists ? ValidationResult.Success : 
            new ValidationResult("This email is already registered");
    }
}