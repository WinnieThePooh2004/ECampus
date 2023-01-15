using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace UniversityTimetable.Shared.Auth;

public class JwtAuthOptions
{
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string Key { get; set; } = default!;
    public SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(Key));
}