using ChatApp.Common.Responses.Interfaces;

namespace ChatApp.Common.Responses.Base;

public class ApiErrorResponse : IApiErrorResponse
{
    public string ErrorKey { get; set; }
    public string Message { get; set; }
    public int Code { get; set; }
    public bool Success { get; set; }
}