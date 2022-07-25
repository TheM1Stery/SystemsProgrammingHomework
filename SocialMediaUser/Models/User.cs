using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SocialMediaUser.Models;

[Index(nameof(Email), IsUnique = true)]
public class User
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string? FirstName { get; set; }
    
    [Required]
    public string? LastName { get; set; }
    
    [Required]
    public DateTime DateOfBirth { get; set; }
    
    [Required]
    public string? PasswordSalt { get; set; }
  
    [Required]
    public string? PasswordHash { get; set; }


    [NotMapped]
    public string? FullName => FirstName + ' ' + LastName;
    
    [Required]
    public string? Email { get; set; }
}