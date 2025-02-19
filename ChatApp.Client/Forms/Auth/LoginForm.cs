using System.ComponentModel.DataAnnotations;

namespace ChatApp.Client.Forms.Auth;

public class LoginForm
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required, MinLength(8), MaxLength(32)]
    public string Password { get; set; }
}