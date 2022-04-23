using AutoMapper;
using BlogProject.Entities.Concrete;
using BlogProject.Entities.DTOs.UserDto;

namespace BlogProject.MvcUI.AutoMapper.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserAddDto, User>();
        }
    }
}
