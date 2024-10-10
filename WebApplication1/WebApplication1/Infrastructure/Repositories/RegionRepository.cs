using Microsoft.EntityFrameworkCore;
using WebApplication1.Core.Entities;
using WebApplication1.Infrastructure.Data;

namespace WebApplication1.Infrastructure.Repositories
{
    public class RegionRepository : IRepository<Region>
    {

        private readonly WalksDbContext _context;

        public RegionRepository(WalksDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(Region entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await _context.Regions.ToListAsync();
        }

        public Task<IEnumerable<Region>> GetByConditionAsync(Func<Region, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Region> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Region entity)
        {
            throw new NotImplementedException();
        }
    }
}
