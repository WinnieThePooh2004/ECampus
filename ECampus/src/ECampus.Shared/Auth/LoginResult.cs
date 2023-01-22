﻿using ECampus.Shared.Enums;

namespace ECampus.Shared.Auth;

public class LoginResult
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public int? GroupId { get; set; }
    public int? TeacherId { get; set; }
    public int? StudentId { get; set; }
    public UserRole Role { get; set; }
}