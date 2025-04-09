using System.Net.Http.Json;
using ChatApp.Common.Responses.Base;
using ChatApp.Common.Dtos.Message;
using Microsoft.AspNetCore.WebUtilities;

namespace ChatApp.Client.Services;

public class MessageService
{
    private readonly HttpClient _httpClient;
    public MessageService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<ApiResponse<IEnumerable<MessageDto>>> GetMessages(long userId)
    {
        var url = QueryHelpers.AddQueryString("/api/messages", "userId", userId.ToString());
        var response = await _httpClient.GetAsync(url);
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<MessageDto>>>();
        return apiResponse!;
    }
}