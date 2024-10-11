using WebApplication1.Core.Entities;
using WebApplication1.DTOs.Response;

namespace WebApplication1.Infrastructure.Repositories
{
    public interface IRegionRepository
    {
        Task<IEnumerable<SearchRegionResponse>> SearchNameCodeRegions(string nameCondition, string urlCondition, string codeCondition);

        Task<Region> DetailRegion(Guid id);
    }
}
