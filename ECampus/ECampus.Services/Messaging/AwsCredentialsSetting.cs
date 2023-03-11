namespace ECampus.Services.Messaging;

public class AwsCredentialsSetting
{
    public const string Key = "AwsCredentials";
    public required string AccessKeyId { get; init; }
    public required string SecretKey { get; init; }
}