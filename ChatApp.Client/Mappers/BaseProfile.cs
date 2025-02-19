using AutoMapper;
using ChatApp.Client.Providers;
using ChatApp.Common.Dtos.User;

namespace ChatApp.Client.Mappers;

public class BaseProfile : Profile
{
    public BaseProfile() : base()
    {
        #region Dtos
        CreateMap<UserDto, ConnectedUser>().ReverseMap();
        #endregion
    }
}