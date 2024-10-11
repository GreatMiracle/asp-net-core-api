using WebApplication1.Core.Entities;
using WebApplication1.DTOs.Response;

namespace WebApplication1.Services.Converter
{
    public class RegionConverter
    {
        public static DetailRegionResponse ConvertRegionToDetailRegionDTO(Region region)
        {
            return new DetailRegionResponse
            {
                Id = region.Id,
                Code = region.Code
            };
        }

        public static DetailRegionResponse MapToDTO1(Region region)
        {
            return new DetailRegionResponse
            {
                Id = region.Id,
                Code = region.Name
            };
        }
    }
}
