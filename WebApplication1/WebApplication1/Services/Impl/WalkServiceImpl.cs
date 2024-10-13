using AutoMapper;
using WebApplication1.Core.Entities;
using WebApplication1.DTOs.Request;
using WebApplication1.DTOs.Response;
using WebApplication1.Infrastructure.Repositories;

namespace WebApplication1.Services.Impl
{
    public class WalkServiceImpl : IWalkService
    {

        private readonly IRepository<RequestBase, Walk> _iRepository;
        private readonly IWalkRepository _walkRepository;
        private readonly IMapper _mapper;

        public WalkServiceImpl(IRepository<RequestBase, Walk> iRepository, IWalkRepository walkRepository, IMapper mapper)
        {
            _iRepository = iRepository;
            _walkRepository = walkRepository;
            _mapper = mapper;
        }

        public async Task<Walk> CreateWalk(CreateWalkRequest req)
        {
            Walk walkReq =_mapper.Map<Walk>(req);
            return await _iRepository.AddAsync(walkReq); 
        }

        public async Task<bool> DeleteWalk(Guid id)
        {
            return await _iRepository.DeleteAsync(id);
        }

        public async Task<DetailWalkResponse?> DetailWalk(Guid id)
        {
            var walk = await _iRepository.GetByIdAsync(id);
            DetailWalkResponse response = _mapper.Map<DetailWalkResponse>(walk);
            return response;
        }

        public async Task<IEnumerable<Walk>> GetAllWalks()
        {
            return await _iRepository.GetAllAsync();
        }

        public async Task<IEnumerable<DetailWalkResponse>> SearchWalks(string? filterOn, string? filterQuery, string? sortBy, string? direction, int pageNumber, int pageSize)
        {
            IEnumerable<Walk> walks = await _walkRepository.SearchNameWalks(filterOn, filterQuery, sortBy, direction, pageNumber, pageSize);
            return _mapper.Map<IEnumerable<DetailWalkResponse>>(walks); ;
        }

        public async Task<Walk> UpdateWalk(Guid id, UpdateWalkRequest request)
        {
            Walk? existingWalk = await _iRepository.GetByIdAsync(id);
            if (existingWalk == null)
            {
                return null;
            }

            // Cập nhật thuộc tính
            existingWalk.Name = request.Name;
            existingWalk.Description = request.Description;
            existingWalk.LenghthInKm = request.LenghthInKm;
            existingWalk.WalkImageUrl = request.WalkImageUrl;
            existingWalk.DifficultyId = request.DifficultyId;
            existingWalk.RegionId = request.RegionId;

            Walk walkResponse = await _iRepository.UpdateAsync(id, existingWalk);
            return walkResponse;
        }
    }
}
