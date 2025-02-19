using System.ComponentModel.DataAnnotations;

namespace ChatApp.Client.Forms.Auth;

public class RegisterForm
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string UserName { get; set; }
    
    [Required, MinLength(8), MaxLength(32)]
    public string Password { get; set; }
    
    [Required, MinLength(8), MaxLength(32), Compare("Password")]
    public string ConfirmPassword { get; set; }
}