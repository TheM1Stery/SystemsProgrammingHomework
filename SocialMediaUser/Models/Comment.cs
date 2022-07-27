using System;
using System.ComponentModel.DataAnnotations;

namespace SocialMediaUser.Models;

public class Comment
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int UserId { get; set; }
    public User? User { get; set; }
    
    public DateTime DateSent { get; set; }
    
    [Required]
    public string? Content { get; set; }
}