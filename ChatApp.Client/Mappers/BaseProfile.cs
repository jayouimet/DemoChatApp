using AutoMapper;
using ChatApp.Client.Forms.Auth;
using ChatApp.Client.Providers;
using ChatApp.Common.Dtos.Auth;
using ChatApp.Common.Dtos.User;

namespace ChatApp.Client.Mappers;

public class BaseProfile : Profile
{
    public BaseProfile() : base()
    {
        #region Dtos
        CreateMap<LoginDto, LoginForm>().ReverseMap();
        CreateMap<RegisterDto, RegisterForm>().ReverseMap();
        
        CreateMap<UserDto, ConnectedUser>().ReverseMap();
        #endregion
    }
}