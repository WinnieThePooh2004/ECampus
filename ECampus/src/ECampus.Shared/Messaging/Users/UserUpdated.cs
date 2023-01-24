﻿namespace ECampus.Shared.Messaging.Users;

public class UserUpdated : IMessage
{
    public required int UserId { get; init; }
    public required string Email { get; init; }
    public required string Username { get; init; }
    public required string Role { get; set; }
}