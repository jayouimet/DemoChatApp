using System.ComponentModel.DataAnnotations;
using ChatApp.Common.Dtos.Abstracts;
using ChatApp.Common.Dtos.Interfaces;

namespace ChatApp.Common.Dtos.Auth;

public class RegisterDto : BaseDto
{
    [Required, MaxLength(30)]
    public string UserName { get; set; }
    [Required, EmailAddress, MaxLength(50)]
    public string Email { get; set; }
    [Required, MinLength(8), MaxLength(32)]
    public string Password { get; set; }

    public override bool ValidateDto()
    {
        return true;
    }
}