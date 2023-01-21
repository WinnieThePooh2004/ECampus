using ECampus.Shared.Enums;
using ECampus.Shared.Models;

namespace ECampus.Tests.Integration.AuthHelpers;

public static class DefaultUsers
{
    public static User Guest => new()
    {
        Username = "username",
        Id = 10,
        Password = "password",
        Email = "email@example.com",
        Role = UserRole.Guest
    };
    
    public static User Admin => new()
    {
        Username = "username",
        Id = 11,
        Password = "password",
        Email = "email@example.com",
        Role = UserRole.Admin
    };
}