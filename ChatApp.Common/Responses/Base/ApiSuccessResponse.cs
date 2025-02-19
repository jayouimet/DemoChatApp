using ChatApp.Common.Responses.Interfaces;

namespace ChatApp.Common.Responses.Base;

public class ApiSuccessResponse<T> : IApiSuccessResponse<T>
{
    public T? Data { get; set; }
    public int Code { get; set; }
    public bool Success { get; set; }
}