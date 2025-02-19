namespace ChatApp.Common.Dtos.Message;

public class MessageDto
{
    public required int ToUserId { get; set; }
    public required string Content { get; set; }
}