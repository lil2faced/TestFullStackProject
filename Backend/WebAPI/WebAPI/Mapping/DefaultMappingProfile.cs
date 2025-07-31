using AutoMapper;
using WebAPI.Application.DTO.UserAPI;
using WebAPI.Core.Entities;

namespace WebAPI.WebAPI.Mapping
{
    public class DefaultMappingProfile : Profile
    {
        public DefaultMappingProfile()
        {
            CreateMap<DTOUserAPIRegistration, UserAPI>()
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.RoleId, opt => opt.Ignore());
            CreateMap<UserAPI, DTOUserAPILogin>()
                .ReverseMap();
            CreateMap<APIRole, DTOAPIRole>()
                .ReverseMap();
            CreateMap<UserAPI, DTOUserAPI>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));
            CreateMap<UserAPI, DTOUserAPIJwt>()
            .ForMember(dest => dest.Role,
                opt => opt.MapFrom(src => new DTOAPIRole { Name = src.Role.Role }));
        }
    }
}
