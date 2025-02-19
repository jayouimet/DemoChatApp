using System.Net.Http.Json;
using ChatApp.Common.Responses.Base;
using ChatApp.Common.Dtos.User;

namespace ChatApp.Client.Services;

public class UserService
{
    private readonly HttpClient _httpClient;
    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<ApiResponse<IEnumerable<UserDto>>> Get()
    {
        var response = await _httpClient.GetAsync("/api/user");
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<UserDto>>>();
        return apiResponse!;
    }
}