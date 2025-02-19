namespace ChatApp.Common.Errors.Abstracts;

public abstract class BaseError(string key, string message) : Exception(message)
{
    public string Key { get; set; } = key;
}