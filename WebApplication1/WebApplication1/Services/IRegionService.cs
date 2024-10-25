using WebApplication1.Core.Entities;
using WebApplication1.DTOs.Request;
using WebApplication1.DTOs.Response;

namespace WebApplication1.Services
{
    public interface IRegionService
    {
        Task<IEnumerable<Region>> GetAllRegions();

        Task<IEnumerable<SearchRegionResponse>> SearchRegions(string nameCondition, string urlCondition, string codeCondition);

        Task<DetailRegionResponse> DetailRegion(Guid id);

        Task<DetailRegionResponse> CreateRegion(CreateRegionRequest region);
        
        Task<DetailRegionResponse> UpdateRegion(Guid id, UpdateRegionRequest request);

        Task<bool> DeleteRegion(Guid id);
    }
}
