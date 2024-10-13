using WebApplication1.Core.Entities;

namespace WebApplication1.DTOs.Response
{
    public class DetailWalkResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Length { get; set; }

        // DTO cho thông tin liên quan
        public Difficulty Difficulty { get; set; }
        public DetailRegionResponse Region { get; set; }
    }
}
