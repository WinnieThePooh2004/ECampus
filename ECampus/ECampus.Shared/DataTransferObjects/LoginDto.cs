﻿namespace ECampus.Shared.DataTransferObjects;

public class LoginDto
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}