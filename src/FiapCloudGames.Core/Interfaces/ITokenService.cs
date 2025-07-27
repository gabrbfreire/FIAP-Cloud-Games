using System.Security.Claims;

namespace FiapCloudGames.Core.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
}