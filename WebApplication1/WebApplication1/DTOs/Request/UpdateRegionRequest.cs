using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs.Request
{
    public class UpdateRegionRequest : RequestBase
    {
        [Required(ErrorMessage = "Code is required.")]
        [StringLength(20, ErrorMessage = "Code cannot be longer than 10 characters.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(250, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Url(ErrorMessage = "Region Image URL must be a valid URL.")]
        public string RegionImageUrl { get; set; }
    }
}
