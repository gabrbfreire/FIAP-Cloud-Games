using Microsoft.AspNetCore.Identity;

namespace FiapCloudGames.Core.Entities;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; } = string.Empty;
}
