namespace ChatApp.Common.Dtos.Interfaces;

public interface IBaseDto
{
    public string ValidationResultMessage { get; }
    public string ValidationResultKey { get; }
    public bool ValidateDto();
}