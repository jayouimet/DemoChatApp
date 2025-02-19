namespace ChatApp.Client.Providers;

public class ConnectedUser
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
}