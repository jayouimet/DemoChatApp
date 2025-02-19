using ChatApp.Common.Responses.Interfaces;

namespace ChatApp.Common.Responses.Base;

public class ApiResponse<T> : IApiErrorResponse, IApiSuccessResponse<T>
{
    public T? Data { get; set; }
    public string? ErrorKey { get; set; }
    public string? Message { get; set; }
    public int Code { get; set; }
    public bool Success { get; set; }
}