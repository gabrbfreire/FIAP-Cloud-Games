namespace FiapCloudGames.Core.Entities;

public class TokenInfo
{
    public TokenInfo(string? token)
    {
        Token = token;
    }

    public int Id { get; set; }
    public string? Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiredAt { get; set; }
}
