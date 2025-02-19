using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ChatApp.Client.Providers;
using ChatApp.Common.Dtos.Auth;
using ChatApp.Common.Responses.Auth;
using ChatApp.Common.Responses.Base;

namespace ChatApp.Client.Handlers;

public class AuthenticatedHttpClientHandler : DelegatingHandler
{
    private readonly AuthStateProvider _authStateProvider;
    private readonly IHttpClientFactory _httpClientFactory;

    public AuthenticatedHttpClientHandler(AuthStateProvider authStateProvider, IHttpClientFactory httpClientFactory)
    {
        _authStateProvider = authStateProvider;
        _httpClientFactory = httpClientFactory;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string? token = await _authStateProvider.GetAccessToken();
        
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (!response.StatusCode.Equals(HttpStatusCode.Unauthorized))
        {
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            if (responseContent.Contains("AccessTokenExpired"))
            {
                bool refreshSuccess = await RefreshTokenAsync(cancellationToken);
                if (refreshSuccess)
                {
                    var clonedRequest = await CloneHttpRequestMessageAsync(request);
                    clonedRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    response = await base.SendAsync(clonedRequest, cancellationToken);
                }
            }
        }

        return response;
    }

    private async Task<bool> RefreshTokenAsync(CancellationToken cancellationToken)
    {
        var currentAccessToken = await _authStateProvider.GetAccessToken();
        var currentRefreshToken = await _authStateProvider.GetRefreshToken();

        if (string.IsNullOrWhiteSpace(currentAccessToken) || string.IsNullOrWhiteSpace(currentRefreshToken))
            return false;
        
        var refreshClient = _httpClientFactory.CreateClient("AnonymousClient");

        var refreshRequest = new HttpRequestMessage(HttpMethod.Post, "/api/auth/refresh");
        RefreshDto refreshPayload = new RefreshDto { AccessToken = currentAccessToken, RefreshToken = currentRefreshToken };
        var json = JsonSerializer.Serialize(refreshPayload);
        refreshRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var refreshResponse = await refreshClient.SendAsync(refreshRequest, cancellationToken);
        if (refreshResponse.IsSuccessStatusCode)
        {
            var content = await refreshResponse.Content.ReadAsStringAsync(cancellationToken);
            var response = JsonSerializer.Deserialize<ApiResponse<AuthResponse>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (response != null)
            {
                await _authStateProvider.Login(response.Data!);
                return true;
            }
        }

        return false;
    }

    private async Task<HttpRequestMessage> CloneHttpRequestMessageAsync(HttpRequestMessage request)
    {
        var clone = new HttpRequestMessage(request.Method, request.RequestUri)
        {
            Version = request.Version
        };

        foreach (var header in request.Headers)
        {
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        if (request.Content != null)
        {
            var ms = new MemoryStream();
            await request.Content.CopyToAsync(ms);
            ms.Position = 0;
            clone.Content = new StreamContent(ms);

            foreach (var header in request.Content.Headers)
            {
                clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        return clone;
    }
}
