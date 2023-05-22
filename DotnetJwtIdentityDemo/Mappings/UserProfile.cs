using AutoMapper;
using DotnetJwtIdentityDemo.DataTransfertObjects;
using DotnetJwtIdentityDemo.Models;

namespace DotnetJwtIdentityDemo.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserRegistrationDto, User>();
        }
    }
}
