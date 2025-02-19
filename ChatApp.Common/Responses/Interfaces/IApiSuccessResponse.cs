namespace ChatApp.Common.Responses.Interfaces;

public interface IApiSuccessResponse<T> : IApiResponse
{
    T? Data { get; set; }   
}