using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Infrastructure.Data
{
    public class ApplicationUser : IdentityUser
    {
        // Thuộc tính tùy chỉnh
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
    }
}
