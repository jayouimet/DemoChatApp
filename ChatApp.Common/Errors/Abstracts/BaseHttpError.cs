using ChatApp.Common.Errors.Interfaces;

namespace ChatApp.Common.Errors.Abstracts;

public abstract class BaseHttpError(string key, string message, int statusCode) : BaseError(key, message), IHttpError
{
    public int StatusCode { get; set; } = statusCode;
}