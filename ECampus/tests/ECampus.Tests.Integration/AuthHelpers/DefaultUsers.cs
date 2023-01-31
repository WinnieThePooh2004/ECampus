using ECampus.Shared.Enums;
using ECampus.Shared.Models;

namespace ECampus.Tests.Integration.AuthHelpers;

public static class DefaultUsers
{
    public static User GetUserByRole(UserRole role)
        => role switch
        {
            UserRole.Admin => Admin,
            UserRole.Guest => Guest,
            UserRole.Student => Student,
            _ => Teacher
        };
    
    private static User Guest => new()
    {
        Username = "guest",
        Id = 10,
        Password = "password",
        Email = "guest@guest.com",
        Role = UserRole.Guest
    };

    private static User Student => new()
    {
        Username = "student",
        Id = 11,
        Password = "password",
        Email = "student@student.com",
        Role = UserRole.Student,
        StudentId = 1
    };
    
    private static User Teacher => new()
    {
        Username = "teacher",
        Id = 11,
        Password = "password",
        Email = "teacher@teacher.com",
        Role = UserRole.Teacher,
        StudentId = 1
    };
    
    private static User Admin => new()
    {
        Username = "admin",
        Id = 13,
        Password = "password",
        Email = "admin@admin.com",
        Role = UserRole.Admin
    };
}