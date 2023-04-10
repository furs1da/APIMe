using APIMe.Entities.Constants;
using APIMe.Entities.DataTransferObjects.Admin.Route;
using APIMe.Entities.DataTransferObjects.Admin.RouteLog;
using APIMe.Entities.DataTransferObjects.Admin.Student;
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
           .ForMember(dest => dest.RouteTypeCrudActionName, opt => opt.MapFrom(src => CrudActions.Actions.FirstOrDefault(item=> item.Id == src.RouteType.CrudId).Action))
           .ForMember(dest => dest.RouteTypeCrudActionId, opt => opt.MapFrom(src => src.RouteType.CrudId));

            CreateMap<RouteDto, Entities.Models.Route>()
          .ForMember(dest => dest.RouteType, opt => opt.Ignore());

            CreateMap<RouteType, RouteTypeDto>().ReverseMap();

            CreateMap<RouteLog, RouteLogDto>().ReverseMap();

            CreateMap<Student, StudentDto>().ReverseMap();
        }
    }
}
