using System.ComponentModel.DataAnnotations;
using ChatApp.Common.Dtos.Abstracts;

namespace ChatApp.Common.Dtos.Auth;

public class RefreshDto : BaseDto
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }

    public override bool ValidateDto()
    {
        if (string.IsNullOrWhiteSpace(AccessToken) || string.IsNullOrWhiteSpace(RefreshToken))
        {
            ValidationResultKey = "InvalidRefreshDto";
            ValidationResultMessage = "Access token and refresh token are required";
            return false;
        }
        return true;
    }
}