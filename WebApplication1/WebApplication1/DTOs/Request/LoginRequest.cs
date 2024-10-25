using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs.Request
{
    public class LoginRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}
