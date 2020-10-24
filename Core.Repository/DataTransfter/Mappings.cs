using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Repository
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<UserDto, Users>();
            CreateMap<Users, UserDto>();
            CreateMap<List<UserDto>, List<Users>>();
            CreateMap<List<Users>, List<UserDto>>();
        }
    }
}
