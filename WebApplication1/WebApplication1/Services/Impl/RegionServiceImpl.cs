using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System.Web.Mvc;
using WebApplication1.Core.Entities;
using WebApplication1.DTOs.Request;
using WebApplication1.DTOs.Response;
using WebApplication1.Infrastructure.Repositories;
using WebApplication1.Infrastructure.Repositories.Impl;
using WebApplication1.Services.Converter;

namespace WebApplication1.Services.Impl
{
    public class RegionServiceImpl : IRegionService
    {
        private readonly IRepository<RequestBase, Region> _iRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionServiceImpl(IRepository<RequestBase, Region> iRepository, IRegionRepository regionRepository, IMapper mapper)
        {
            _iRepository = iRepository;
            _regionRepository = regionRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<Region>> GetAllRegions()
        {
            IEnumerable<Region> listRegions = await _iRepository.GetAllAsync();
          

            return listRegions;

            //var regions = new List<Region>
            //{
            //    new Region
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Auckland",
            //        Code = "AKL",
            //        ImageUrl = "https://example.com/auckland.jpg"
            //    },
            //    new Region
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Wellington",
            //        Code = "WLG",
            //        ImageUrl = "https://example.com/wellington.jpg"
            //    }
            //};
            //return regions;
        }

        public async Task<IEnumerable<SearchRegionResponse>> SearchRegions
            (string nameCondition, string urlCondition, string codeCondition)
        {
            IEnumerable<SearchRegionResponse> listRegions = await _regionRepository
                .SearchNameCodeRegions(nameCondition, urlCondition, codeCondition);

            return listRegions;
        }

        public async Task<DetailRegionResponse> DetailRegion(Guid id)
        {
            var region = await _regionRepository.DetailRegion(id);

            // Mapping domain model to DTO
            //var regionDTO = RegionConverter.ConvertRegionToDetailRegionDTO(region);

            var regionDTO = _mapper.Map<DetailRegionResponse>(region);

            return regionDTO;
        }

        public async Task<DetailRegionResponse> CreateRegion(CreateRegionRequest req)
        {
            var region = new Region
            {
                Code = req.Code,
                Name = req.Name,
                ImageUrl = req.RegionImageUrl
            };

            Region regionRes = await _iRepository.AddAsync(region);
            return new DetailRegionResponse
            {
                Id = regionRes.Id,
                Code = regionRes.Code,
            }; ;
        }

        public async Task<DetailRegionResponse> UpdateRegion(Guid id, UpdateRegionRequest request)
        {
            var regionIsExit = await _iRepository.GetByIdAsync(id);

            // Kiểm tra nếu khu vực không tồn tại
            if (regionIsExit == null)
            {
                return null; // Trả về null để controller có thể xử lý
            }

            regionIsExit.Code = request.Code;
            regionIsExit.Name = request.Name;
            regionIsExit.ImageUrl = request.RegionImageUrl;

            Region region = await _iRepository.UpdateAsync(id, regionIsExit);
            return new DetailRegionResponse
            {
                Id = region.Id,
                Code = region.Code,
            }; ;
        }

        public async Task<bool> DeleteRegion(Guid id)
        {
            return await _iRepository.DeleteAsync(id);
        }
    }
}
