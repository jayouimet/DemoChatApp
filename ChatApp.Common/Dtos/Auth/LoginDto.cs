using System.ComponentModel.DataAnnotations;
using ChatApp.Common.Dtos.Abstracts;

namespace ChatApp.Common.Dtos.Auth;

public class LoginDto : BaseDto
{
    [Required, EmailAddress, MaxLength(50)]
    public string Email { get; set; }
    [Required, MinLength(8), MaxLength(32)]
    public string Password { get; set; }

    public override bool ValidateDto()
    {
        return true;
    }
}