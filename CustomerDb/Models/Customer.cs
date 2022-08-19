using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CustomerDb.Models;

public partial class Customer : ObservableValidator
{
    public int Id { get; set; }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Field FirstName must not have letters")]
    private string? _firstName;
    

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Field LastName must not have letters")]
    private string? _lastName;
    

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [EmailAddress]
    private string? _email;
    

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private Gender? _gender;
    

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [Range(16, 120)]
    private int? _age;
}