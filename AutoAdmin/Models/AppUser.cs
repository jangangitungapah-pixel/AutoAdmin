using Microsoft.AspNetCore.Identity;

namespace AutoAdmin.Models;

public class AppUser : IdentityUser
{
    public string? FullName { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<Business> Businesses { get; set; } = new List<Business>();
}