using Azure.Core;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Sprache;
using WebApplication1.Core.Entities;
using WebApplication1.DTOs;
using WebApplication1.DTOs.Request;
using WebApplication1.DTOs.Response;
using WebApplication1.Infrastructure.Data;

namespace WebApplication1.Infrastructure.Repositories.Impl
{
    public class RegionRepository : IRepository<RegionRequestBase, Region>, IRegionRepository
    {

        private readonly WalksDbContext _context;

        public RegionRepository(WalksDbContext context)
        {
            _context = context;
        }

        public async Task<Region> AddAsync(Region entity)
        {
            try
            {
                await _context.Regions.AddAsync(entity);
                await _context.SaveChangesAsync();

                return entity; 
            }
            catch (DbUpdateException ex)
            {
                // Xử lý lỗi khi không thể lưu đối tượng vào cơ sở dữ liệu
                throw new Exception("Có lỗi xảy ra khi lưu Region. Vui lòng thử lại sau.", ex);
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác
                throw new Exception("Có lỗi không xác định xảy ra. Vui lòng thử lại sau.", ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var region = await _context.Regions.FirstOrDefaultAsync(x => x.Id == id);

                if (region == null)
                {
                    return false;
                }

                _context.Regions.Remove(region);
                await _context.SaveChangesAsync();

                return true; 
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Xử lý khi có lỗi cập nhật đồng thời
                // Bạn có thể log lỗi hoặc xử lý theo cách khác
                Console.WriteLine($"Concurrency error: {ex.Message}");
                return false;
            }
            catch (DbUpdateException ex)
            {
                // Xử lý lỗi khi lưu thay đổi không thành công
                Console.WriteLine($"Update error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await _context.Regions.ToListAsync();
        }

        public Task<IEnumerable<Region>> GetByConditionAsync(Func<RegionRequestBase, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Region> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Region> UpdateAsync(Guid id, RegionRequestBase request)
        {
            var region = await _context.Regions.FirstOrDefaultAsync(x => x.Id == id);

            // Kiểm tra nếu khu vực không tồn tại
            if (region == null)
            {
                return null; // Trả về null để controller có thể xử lý
            }

            // Cập nhật thông tin khu vực
            // Bạn có thể cast entity về kiểu UpdateRegionRequest nếu cần
            if (request is UpdateRegionRequest updateRequest)
            {
                region.Code = updateRequest.Code;
                region.Name = updateRequest.Name;
                region.ImageUrl = updateRequest.RegionImageUrl;
            }

            try
            {
                // Lưu thay đổi
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Có lỗi xảy ra khi update thông tin Regionc. Vui lòng thử lại sau.");
            }

            return region;
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

        public async Task<IEnumerable<SearchRegionResponse>> GetRegionsNameByNativeQueryAsync(string condition)
        {
            string sqlQuery = "SELECT Id, Name FROM Regions WHERE Name LIKE {0}"; // Chỉ lấy Id và Name
            var regions = await _context.Regions
                .FromSqlRaw(sqlQuery, $"%{condition}%")
                .Select(r => new SearchRegionResponse
                {
                    Id = r.Id, // Gán Id từ lớp Region
                    FirstName = r.Name // Gán Name từ lớp Region
                })
                .ToListAsync();

            return regions;
        }

        public async Task<IEnumerable<SearchRegionResponse>> SearchNameCodeRegions(string nameCondition, string urlCondition, string codeCondition)
        {
            // Sử dụng SqlParameter để tránh SQL Injection lib:Microsoft.Data.SqlClient
            var parameters = new List<NpgsqlParameter>{
                    new NpgsqlParameter("@name", $"%{nameCondition}%"),
                    new NpgsqlParameter("@imageUrl", urlCondition),
                    new NpgsqlParameter("@code", $"%{codeCondition}%")
                };

            var query = @"
                SELECT r.""Id"", r.""Name"" as firstname, r.""Code"", r.""ImageUrl""
                FROM ""Regions"" r
                WHERE (@name IS NULL OR r.""Name"" ILIKE @name)
                  AND (@code IS NULL OR r.""Code"" ILIKE @code)";

            //var result = await _context.Regions
            //   .FromSqlRaw(query, parameters.ToArray())
            //    .AsNoTracking()
            //  .Select(r => new RegionDto
            //  {
            //      Id = r.Id,
            //      FirstName = Utils.PropertyValueHelper.GetStringValueOrDefault(r, "firstname")

            //  })
            //    .ToListAsync();

            var result = await _context.Database.SqlQueryRaw<SearchRegionDTO>(query, parameters.ToArray()).ToListAsync();

            var regions = result.Select(r => new SearchRegionResponse
            {
                Id = r.Id,
                FirstName = r.FirstName ?? string.Empty,
            });

            return regions;
        }

        public async Task<Region> DetailRegion(Guid id)
        {
            return await _context.Regions.FindAsync(id);
          
        }
    }
}
