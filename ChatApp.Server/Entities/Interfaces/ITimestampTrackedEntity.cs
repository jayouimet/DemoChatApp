namespace ChatApp.Server.Entities.Interfaces;

public interface ITimestampTrackedEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}