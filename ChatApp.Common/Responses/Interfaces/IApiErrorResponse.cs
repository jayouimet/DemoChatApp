namespace ChatApp.Common.Responses.Interfaces;

public interface IApiErrorResponse : IApiResponse
{
    public string ErrorKey { get; set; }
    public string Message { get; set; }
}