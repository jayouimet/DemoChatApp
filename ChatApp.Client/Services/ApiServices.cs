namespace ChatApp.Client.Services;

public class ApiServices
{
    private readonly HttpClient _authenticatedClient;
    private readonly HttpClient _anonymousClient;

    public readonly AuthService Auth;
    public readonly UserService User;
    public readonly MessageService Message;

    public ApiServices(IHttpClientFactory httpClientFactory)
    {
        _authenticatedClient = httpClientFactory.CreateClient("AuthenticatedClient");
        _anonymousClient = httpClientFactory.CreateClient("AnonymousClient");

        Auth = new AuthService(_anonymousClient);
        User = new UserService(_authenticatedClient);
        Message = new MessageService(_authenticatedClient);
    }
    
    public HttpClient AuthenticatedClient => _authenticatedClient;
    public HttpClient AnonymousClient => _anonymousClient;
}