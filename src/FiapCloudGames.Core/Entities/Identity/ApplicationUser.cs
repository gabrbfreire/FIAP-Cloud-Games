using Microsoft.AspNetCore.Identity;

namespace FiapCloudGames.Core.Entities.Identity;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; } = string.Empty;
}
