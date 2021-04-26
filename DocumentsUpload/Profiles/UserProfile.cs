using AutoMapper;
using DocumentsUpload.Entities;
using DocumentsUpload.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentsUpload.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserCreationDto, User>();
            CreateMap<User, UserDto>();
        }
    }
}
