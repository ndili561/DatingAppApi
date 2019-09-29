using API.DTO;
using AutoMapper;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(op => op.UserName))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(op => op.PassWord));
            CreateMap<PhotoDTO, Photo>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(op => op.id));
              
        }
        

    }
}
