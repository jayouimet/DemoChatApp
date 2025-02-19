using AutoMapper;
using ChatApp.Common.Dtos.Auth;
using ChatApp.Common.Dtos.User;
using ChatApp.Server.Entities;

namespace ChatApp.Server.Mappers;

public class BaseProfile : Profile
{
    public BaseProfile() : base()
    {
        #region Dtos
        CreateMap<RegisterDto, User>().ReverseMap();
        
        CreateMap<UserDto, User>().ReverseMap();
        #endregion
    }
}