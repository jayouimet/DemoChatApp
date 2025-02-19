using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChatApp.Server.Entities.Abstracts;

namespace ChatApp.Server.Entities;

[Table("Messages")]
public class Message : TimestampTrackedEntity
{
    [Required, MaxLength(512)]
    public required string Content { get; set; }
    public DateTime SentAt { get; set; }
    
    public long FromUserId { get; set; }
    public long? ToUserId { get; set; }
    
    [ForeignKey("FromUserId")]
    public virtual User FromUser { get; set; }
    [ForeignKey("ToUserId")]
    public virtual User ToUser { get; set; }
}