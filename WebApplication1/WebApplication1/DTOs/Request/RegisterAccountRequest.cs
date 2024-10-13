using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs.Request
{
    public class RegisterAccountRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string[] Roles { get; set; } // Optional roles
    }
}
