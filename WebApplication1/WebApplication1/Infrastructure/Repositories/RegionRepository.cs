using Microsoft.EntityFrameworkCore;
using WebApplication1.Core.Entities;
using WebApplication1.DTOs;
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

        public async Task<int> ExecuteCustomQueryAsync()
        {
            string sqlQuery = "UPDATE Regions SET Name = 'UpdatedName' WHERE Id = {0}";
            return await _context.Database.ExecuteSqlRawAsync(sqlQuery, 1); // 1 là ID của vùng
        }

        public async Task<IEnumerable<Region>> GetRegionsByNativeQueryAsync(string condition)
        {
            string sqlQuery = "SELECT * FROM Regions WHERE Name LIKE {0}";
            return await _context.Regions.FromSqlRaw(sqlQuery, $"%{condition}%").ToListAsync();
        }

        public async Task<IEnumerable<RegionDto>> GetRegionsNameByNativeQueryAsync(string condition)
        {
            string sqlQuery = "SELECT Id, Name FROM Regions WHERE Name LIKE {0}"; // Chỉ lấy Id và Name
            var regions = await _context.Regions
                .FromSqlRaw(sqlQuery, $"%{condition}%")
                .Select(r => new RegionDto
                {
                    Id = r.Id, // Gán Id từ lớp Region
                    Name = r.Name // Gán Name từ lớp Region
                })
                .ToListAsync();

            return regions;
        }
    }
}
