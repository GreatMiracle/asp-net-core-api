using System.Globalization;
using WebApplication1.Core.Entities;
using WebApplication1.DTOs.Response;

namespace WebApplication1.Infrastructure.Repositories
{
    public interface IWalkRepository
    {
        Task<IEnumerable<Walk>> SearchNameWalks(string? filterOn, string? filterQuery, string? sortBy, string? direction, int pageNumber, int pageSize);
    }
}
