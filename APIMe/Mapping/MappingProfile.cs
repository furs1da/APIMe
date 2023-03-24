using APIMe.Entities.DataTransferObjects.Admin.Route;
using APIMe.Entities.Models;
using AutoMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace APIMe.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Entities.Models.Route, RouteDto>()
           .ForMember(dest => dest.RouteTypeName, opt => opt.MapFrom(src => src.RouteType.Name))
           .ForMember(dest => dest.RouteTypeResponseCode, opt => opt.MapFrom(src => src.RouteType.ResponseCode))
           .ForMember(dest => dest.RouteTypeCrudActionId, opt => opt.MapFrom(src => src.RouteType.CrudId));

            CreateMap<RouteType, RouteTypeDto>().ReverseMap();
        }
    }
}
