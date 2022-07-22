using System;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SocialMediaUser.Models;

public partial class RegistrationModel : ObservableValidator
{
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
    [Required]
    private string? _email;

}