using AutoMapper;
using WebApplication1.Core.Entities;
using WebApplication1.DTOs.Request;
using WebApplication1.DTOs.Response;

namespace WebApplication1.Services.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //Region Mapper
            CreateMap<DetailRegionResponse, Region>().ReverseMap();

            //Walk Mapper
            CreateMap<CreateWalkRequest, Walk>();

            CreateMap<Walk, DetailWalkResponse>()
               .ForMember(dest => dest.Length, opt => opt.MapFrom(src => src.LenghthInKm))  // Đổi tên trường Length
               .ForMember(dest => dest.Difficulty, opt => opt.MapFrom(src => src.Difficulty)) // Ánh xạ Difficulty
               .ForMember(dest => dest.Region, opt => opt.MapFrom(src => new DetailRegionResponse
               {
                   Id = src.Region.Id, // Chỉ ánh xạ các thuộc tính cần thiết
                   Code = src.Region.Code
               }));

        }

    }


}
