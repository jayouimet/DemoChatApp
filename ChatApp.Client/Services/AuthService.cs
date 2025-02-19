using System.Net.Http.Json;
using ChatApp.Common.Dtos.Auth;
using ChatApp.Common.Responses.Auth;
using ChatApp.Common.Responses.Base;

namespace ChatApp.Client.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResponse<AuthResponse>> Login(LoginDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/auth/login", dto);
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponse>>();
        return apiResponse!;
    }

    public async Task<ApiResponse<AuthResponse>> Register(RegisterDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/auth/register", dto);
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponse>>();
        return apiResponse!;
    }
    
    public async Task<ApiResponse<AuthResponse>> Refresh(RefreshDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/auth/refresh", dto);
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponse>>();
        return apiResponse!;
    }
}