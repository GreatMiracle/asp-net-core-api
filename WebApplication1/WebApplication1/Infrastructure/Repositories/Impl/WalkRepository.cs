using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq.Expressions;
using WebApplication1.Core.Entities;
using WebApplication1.Core.Utils;
using WebApplication1.DTOs.Request;
using WebApplication1.DTOs.Response;
using WebApplication1.Infrastructure.Data;

namespace WebApplication1.Infrastructure.Repositories.Impl
{
    public class WalkRepository : IRepository<RequestBase, Walk>, IWalkRepository
    {

        private readonly WalksDbContext _context;

        public WalkRepository(WalksDbContext context)
        {
            _context = context;
        }

        public async Task<Walk> AddAsync(Walk entity)
        {
            try
            {
                await _context.Walks.AddAsync(entity);
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
                var walkIsExit = await _context.Walks.FirstOrDefaultAsync(x => x.Id == id);

                if (walkIsExit == null)
                {
                    return false;
                }
                _context.Walks.Remove(walkIsExit);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine($"Concurrency error: {ex.Message}");
                return false;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Update error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await _context.Walks
                .Include("Difficulty") //Phương Pháp Chuỗi (String-Based Syntax)
                .Include(w => w.Region) //Sử Dụng Lambda Expressions
                .ToListAsync();
        }

        public Task<IEnumerable<Walk>> GetByConditionAsync(Func<RequestBase, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await _context.Walks
            .Include(w => w.Difficulty)
            .Include(w => w.Region)
            .FirstOrDefaultAsync(w => w.Id == id); 
        }

        public async Task<IEnumerable<Walk>> SearchNameWalks(string? filterOn, string? filterQuery, string? sortBy, string? direction, int pageNumber, int pageSize)
        {
            var query = _context.Walks.AsQueryable();

            //Cách JOIN của LINQ:
            //var query = from w in _context.Works
            //            join a in _context.Authors on new { w.AuthorId, w.Country } equals new { a.Id, a.Country }
            //            select new WorkWithAuthorInfo
            //            {
            //                WorkId = w.Id,
            //                WorkName = w.Name,
            //                AuthorName = a.Name,
            //                Description = w.Description
            //            };

            //var result = context.Works
            //                    .Join(
            //                context.Authors,
            //                w => new { w.AuthorId, w.Country },
            //                a => new { a.Id, a.Country },
            //                (w, a) => new { w, a })
            //            .Select(x => new { x.w, x.a });


            // Thực hiện lọc điều kiện nếu cần
            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                // Lọc theo tên công việc
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    //Cách 1:
                    //query = query.Where(w => w.Name.ToLower().Contains(filterQuery.ToLower()));

                    //Cách 2:
                    var lowerFilterQuery = filterQuery.ToLower();
                    query = query.Where(w => EF.Functions.Like(w.Name.ToLower(), $"%{lowerFilterQuery}%"));
                }
              
            }

            // Sắp xếp 
            if (sortBy.Equals("NameWalk", StringComparison.OrdinalIgnoreCase))
            {
                query = direction.Equals("asc", StringComparison.OrdinalIgnoreCase)
                    ? query.OrderBy(w => w.Name)
                    : query.OrderByDescending(w => w.Name);
            }
            else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
            {
                query = direction.Equals("asc", StringComparison.OrdinalIgnoreCase)
                    ? query.OrderBy(w => w.LenghthInKm)
                    : query.OrderByDescending(w => w.LenghthInKm);
            }
            else
            {
                query = query.OrderByProperty(sortBy, direction);
            }

            // Sử dụng phương thức Paginate để phân trang
            return await query.Paginate(pageNumber, pageSize).ToListAsync();
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk request)
        {
            try
            {
                // Đảm bảo rằng đối tượng region đang được tracking bởi DbContext
                _context.Walks.Update(request);

                // Lưu thay đổi
                await _context.SaveChangesAsync();
                return request;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Có lỗi xảy ra khi update thông tin Regionc. Vui lòng thử lại sau.");
            }
        }
    }
}
