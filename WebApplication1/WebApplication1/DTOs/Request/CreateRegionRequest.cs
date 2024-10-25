using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs.Request
{
    public class CreateRegionRequest : RequestBase
    {
        [Required(ErrorMessage = "Code is required.")]
        [StringLength(20, ErrorMessage = "Code cannot be longer than 10 characters.")]
        [MinLength(3, ErrorMessage = "Code must be at least 3 characters long")]
        //[MaxLength(20, ErrorMessage = "Code can only be 3 characters long")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(250, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Url(ErrorMessage = "Region Image URL must be a valid URL.")]
        public string RegionImageUrl { get; set; }
    }
}
