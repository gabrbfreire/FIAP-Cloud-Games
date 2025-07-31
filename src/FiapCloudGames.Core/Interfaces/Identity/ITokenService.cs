using System.Security.Claims;

namespace FiapCloudGames.Core.Interfaces.Identity;

public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
}