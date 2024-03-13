using System.ComponentModel.DataAnnotations;

namespace FinanceFolio.Models;

public class User
{
    [Required]
    public int UserId { get; set; }
    [Required]
    public string Username { get; set; }
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    
    [Required, Compare(nameof(Password), ErrorMessage = "The passwords didn't match.")]
    public required string ConfirmPassword { get; set; }
}