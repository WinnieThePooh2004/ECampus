using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ECampus.Domain.Auth;

public class JwtAuthOptions
{
    public const string ConfigKey = "JwtAuthOptions";
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string Key { get; set; } = default!;
    public SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(Key));
}