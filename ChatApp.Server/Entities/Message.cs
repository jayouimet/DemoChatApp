using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
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
    public required User FromUser { get; set; }
    [ForeignKey("ToUserId")]
    public required User ToUser { get; set; }

    public static Expression<Func<Message, bool>> IsUserConcerned(long? userId)
    {
        return m => m.FromUserId == userId || m.ToUserId == userId;
    }
    
    public static Expression<Func<Message, bool>> IsConcerned(long? userId1, long? userId2)
    {
        return m => (m.FromUserId == userId1 || m.ToUserId == userId1) &&
                    (m.FromUserId == userId2 || m.ToUserId == userId2);
    }
}