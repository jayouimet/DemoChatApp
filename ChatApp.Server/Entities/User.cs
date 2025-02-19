using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChatApp.Server.Entities.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Server.Entities;

[Table("Users")]
public class User : TimestampTrackedEntity
{
    [Required, Unicode(false), MaxLength(30)]
    public required string UserName { get; set; }
    [Required, MaxLength(50), EmailAddress]
    public required string Email { get; set; }
    [Required, MaxLength(255)]
    public required string PasswordHash { get; set; }
    
    [MaxLength(255)]
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
    
    
    public virtual ICollection<Message> SentMessages { get; set; } = new List<Message>();
    public virtual ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();
}