using WebApplication1.Core.Entities;
using WebApplication1.Infrastructure.Repositories;

namespace WebApplication1.Services.Impl
{
    public class RegionService : IRegionService
    {
        private readonly IRepository<Region> _regionRepository;
        public RegionService(IRepository<Region> regionRepository)
        {
            _regionRepository = regionRepository;
        }
        public async Task<IEnumerable<Region>> GetAllRegions()
        {
            return await _regionRepository.GetAllAsync();
        }
    }
}
