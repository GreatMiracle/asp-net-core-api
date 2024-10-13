using Microsoft.AspNetCore.Mvc;
using WebApplication1.Core.Entities;
using WebApplication1.DTOs.Request;
using WebApplication1.DTOs.Response;

namespace WebApplication1.Services
{
    public interface IWalkService
    {
        Task<IEnumerable<Walk>> GetAllWalks();

        Task<IEnumerable<DetailWalkResponse>> SearchWalks(
            string? filterOn, string? filterQuery
            , string? sortBy,string? direction
            , int pageNumber,int pageSize);

        Task<DetailWalkResponse> DetailWalk(Guid id);

        Task<Walk> CreateWalk(CreateWalkRequest region);

        Task<Walk> UpdateWalk(Guid id, UpdateWalkRequest request);

        Task<bool> DeleteWalk(Guid id);

    }
}
