using FiapCloudGames.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace FiapCloudGames.Core.Interfaces.Identity;

public interface IAuthService
{
    Task<IdentityResult> SignupAsync(string name, string email, string password);
    Task<TokenInfo?> LoginAsync(string email, string password);
    Task<IdentityResult> DeleteAsync(string email);
}
