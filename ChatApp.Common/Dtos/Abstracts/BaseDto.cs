using ChatApp.Common.Dtos.Interfaces;

namespace ChatApp.Common.Dtos.Abstracts;

public abstract class BaseDto : IBaseDto
{
    public string ValidationResultMessage { get; protected set; } = string.Empty;
    public string ValidationResultKey { get; protected set; } = string.Empty;

    public virtual bool ValidateDto()
    {
        return true;
    }
}