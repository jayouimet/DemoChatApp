namespace ChatApp.Common.Responses.Interfaces;

public interface IApiResponse
{
    public int Code { get; set; }
    public bool Success { get; set; }
}