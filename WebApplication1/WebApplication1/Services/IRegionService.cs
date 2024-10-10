using WebApplication1.Core.Entities;

namespace WebApplication1.Services
{
    public interface IRegionService
    {
        Task<IEnumerable<Region>> GetAllRegions();
    }
}
