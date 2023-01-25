﻿namespace ECampus.Messaging.Messages;

public class UserUpdated : ISqsMessage
{
    public required int UserId { get; init; }
    public required string Email { get; init; }
    public required string Username { get; init; }
    public required string Role { get; set; }
}