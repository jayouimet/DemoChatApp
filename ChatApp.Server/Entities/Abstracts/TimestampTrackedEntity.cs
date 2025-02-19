using ChatApp.Server.Entities.Interfaces;

namespace ChatApp.Server.Entities.Abstracts;

public abstract class TimestampTrackedEntity : BaseEntity, ITimestampTrackedEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}