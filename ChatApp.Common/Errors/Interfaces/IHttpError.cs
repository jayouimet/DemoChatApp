namespace ChatApp.Common.Errors.Interfaces;

public interface IHttpError
{
    public int StatusCode { get; set; }
}