using AutoMapper;
using MinimalAPI.Dtos;
using MinimalAPI.Models;

namespace MinimalAPI.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            //source => Target
            CreateMap<User, UserReadDto>();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>();
        }
    }
}
