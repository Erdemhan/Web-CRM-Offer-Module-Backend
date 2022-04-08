using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using crmweb.Data.Entities;
using crmweb.Models.UserModels;

namespace crmweb.Models.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserInfo>()
                .ReverseMap();
            CreateMap<User, UserRequestInfo>()
                .ReverseMap();
            CreateMap<UserUpdateInfo,User>()
                .ReverseMap();
        }
    }
}
