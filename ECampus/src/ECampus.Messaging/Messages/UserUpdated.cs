﻿namespace ECampus.Messaging.Messages;

public class UserUpdated : ISqsMessage
{
    public required string Email { get; init; }
    public required string Username { get; init; }
}